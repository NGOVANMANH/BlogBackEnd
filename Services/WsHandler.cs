using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using api.DTOs;
using System.Text.Json;

namespace api.Services;

public class WsHandler
{
    private readonly ConcurrentDictionary<int, WebSocket> _sockets = new ConcurrentDictionary<int, WebSocket>();
    private readonly ILogger<WsHandler> _logger;
    private const int SYSTEM = -1;

    public WsHandler(ILogger<WsHandler> logger)
    {
        _logger = logger;
    }

    public async Task AddSocketAsync(int userId, WebSocket socket)
    {
        if (_sockets.TryAdd(userId, socket))
        {
            _logger.LogInformation($"{userId} has joined the chat.");
            await BroadcastMessageAsync(new ChatMessageDTO
            {
                UserId = SYSTEM,
                Content = $"{userId} has joined the chat.",
                Timestamp = DateTime.UtcNow
            });
        }
        else
        {
            _logger.LogWarning($"Failed to add socket for {userId}");
        }
    }

    public async Task RemoveSocketAsync(int userId)
    {
        if (_sockets.TryRemove(userId, out var socket))
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketHandler", CancellationToken.None);
            _logger.LogInformation($"{userId} has left the chat.");
            await BroadcastMessageAsync(new ChatMessageDTO
            {
                UserId = SYSTEM,
                Content = $"{userId} has left the chat.",
                Timestamp = DateTime.UtcNow
            });
        }
        else
        {
            _logger.LogWarning($"Failed to remove socket for {userId}");
        }
    }

    public async Task SendMessageAsync(int userId, ChatMessageDTO chatMessageDTO)
    {
        if (_sockets.TryGetValue(userId, out var socket) && socket.State == WebSocketState.Open)
        {
            var chatMessageJson = JsonSerializer.Serialize(chatMessageDTO);
            var buffer = Encoding.UTF8.GetBytes(chatMessageJson);

            try
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending message to {userId}: {ex.Message}");
            }
        }
        else
        {
            _logger.LogWarning($"Socket for {userId} is not open or does not exist.");
        }
    }

    public async Task ReceiveMessageAsync(int userId)
    {
        if (_sockets.TryGetValue(userId, out var socket))
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            try
            {
                do
                {
                    result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (!result.CloseStatus.HasValue)
                    {
                        var chatMessageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var chatMessageDTO = JsonSerializer.Deserialize<ChatMessageDTO>(chatMessageJson);

                        _logger.LogInformation($"Sever recieved: {chatMessageDTO?.UserId} => {chatMessageDTO?.Content}");

                        // Phát lại tin nhắn cho tất cả các kết nối khác
                        foreach (var s in _sockets.Where(s => s.Key != userId))
                        {
                            if (s.Value.State == WebSocketState.Open)
                            {
                                await SendMessageAsync(s.Key, chatMessageDTO!);
                            }
                        }
                    }
                } while (!result.CloseStatus.HasValue);

                await RemoveSocketAsync(userId);
            }
            catch (WebSocketException ex)
            {
                _logger.LogError($"WebSocket error while receiving message from {userId}: {ex.Message}");
                await RemoveSocketAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error receiving message from {userId}: {ex.Message}");
                await RemoveSocketAsync(userId);
            }
        }
        else
        {
            _logger.LogWarning($"Socket for {userId} does not exist.");
        }
    }


    private async Task BroadcastMessageAsync(ChatMessageDTO chatMessageDTO)
    {
        var chatMessageJson = JsonSerializer.Serialize(chatMessageDTO);
        var buffer = Encoding.UTF8.GetBytes(chatMessageJson);

        foreach (var socket in _sockets.Values)
        {
            if (socket.State == WebSocketState.Open)
            {
                try
                {
                    await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error broadcasting message: {ex.Message}");
                }
            }
        }
    }
}
