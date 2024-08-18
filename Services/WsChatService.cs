using System.Collections.Concurrent;
using System.Net.WebSockets;
using api.DTOs.Chat;
using api.Interfaces;
using api.Utils;

namespace api.Services
{
    public class WsChatService : IWsChatService
    {
        private readonly ConcurrentDictionary<int, WebSocket> _connections = new ConcurrentDictionary<int, WebSocket>();
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public WsChatService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public bool AddConnection(int userId, WebSocket webSocket)
        {
            return _connections.TryAdd(userId, webSocket);
        }

        public async Task HandleWebSocketCommunicationAsync(int userId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _chatService = scope.ServiceProvider.GetRequiredService<IChatService>();

                if (_connections.TryGetValue(userId, out var webSocket))
                {
                    var msgBytes = new byte[1024 * 4];
                    WebSocketReceiveResult result;

                    while (webSocket.State == WebSocketState.Open)
                    {
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(msgBytes), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocket client", CancellationToken.None);
                            RemoveConnection(userId);
                        }
                        else if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var msg = WebSocketUtil.ParseMessage<MessageRequest>(new ArraySegment<byte>(msgBytes, 0, result.Count));
                            if (msg is not null)
                            {
                                await HandleMessageAsync(userId, msg, _chatService);
                            }
                        }
                    }
                }
            }
        }

        private async Task HandleMessageAsync(int senderId, MessageRequest msg, IChatService _chatService)
        {
            var msgDTO = Mappers.MessageMapper.ToDTO(msg, senderId);

            var existingRoom = await _chatService.GetRoomByIdAsync(msg.RoomId);

            if (existingRoom is not null)
            {
                var newMsgDTO = await _chatService.AddMessageAsync(msgDTO);
                if (newMsgDTO is null) return;
                await _chatService.AddMessageToRoomAsync(newMsgDTO.RoomId, newMsgDTO.Id!);
                var members = existingRoom.Members;
                foreach (var ws in _connections.Where(c => members.Contains(c.Key) && c.Key != senderId).Select(c => c.Value))
                {
                    if (ws.State == WebSocketState.Open)
                    {
                        var msgJson = WebSocketUtil.CreateMessageSegment<MessageDTO>(newMsgDTO);
                        await ws.SendAsync(msgJson, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
        }

        public bool RemoveConnection(int userId)
        {
            return _connections.TryRemove(userId, out _);
        }
    }
}