using System.Net.WebSockets;

namespace api.Interfaces
{
    public interface IWsChatService
    {
        bool AddConnection(int userId, WebSocket webSocket);
        bool RemoveConnection(int userId);
        Task HandleWebSocketCommunicationAsync(int userId);
    }
}