using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using api.DTOs;
using Newtonsoft.Json;

namespace api.Services;

public class WsHandler
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

    public void AddSocket(string id, WebSocket socket)
    {
        _sockets.TryAdd(id, socket);
        // Gửi thông báo khi một client mới tham gia
        BroadcastMessageAsync(new ChatMessageDTO
        {
            Sender = "System",
            Message = $"{id} has joined the chat.",
            Timestamp = DateTime.UtcNow
        }).Wait();
    }

    public async Task RemoveSocket(string id)
    {
        if (_sockets.TryRemove(id, out var socket))
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketHandler", CancellationToken.None);
            // Gửi thông báo khi một client rời đi
            await BroadcastMessageAsync(new ChatMessageDTO
            {
                Sender = "Sytem",
                Message = $"{id} has left the chat.",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    public async Task SendMessageAsync(string id, ChatMessageDTO chatMessageDTO)
    {
        if (_sockets.TryGetValue(id, out var socket) && socket.State == WebSocketState.Open)
        {
            var chatMessageJson = JsonConvert.SerializeObject(chatMessageDTO);
            var buffer = Encoding.UTF8.GetBytes(chatMessageJson);
            await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public async Task ReceiveMessageAsync(string id)
    {
        if (_sockets.TryGetValue(id, out var socket))
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            do
            {
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (!result.CloseStatus.HasValue)
                {
                    var chatMessageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var chatMessageDTO = JsonConvert.DeserializeObject<ChatMessageDTO>(chatMessageJson);
                    // Phát lại tin nhắn cho tất cả các kết nối khác
                    foreach (var s in _sockets.Where(s => s.Key != id))
                    {
                        if (s.Value.State == WebSocketState.Open)
                        {
                            await SendMessageAsync(s.Key, chatMessageDTO!);
                        }
                    }
                }
            } while (!result.CloseStatus.HasValue);

            await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            await RemoveSocket(id);
        }
    }

    private async Task BroadcastMessageAsync(ChatMessageDTO chatMessageDTO)
    {
        var chatMessageJson = JsonConvert.SerializeObject(chatMessageDTO);
        var buffer = Encoding.UTF8.GetBytes(chatMessageJson);
        foreach (var socket in _sockets.Values)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}