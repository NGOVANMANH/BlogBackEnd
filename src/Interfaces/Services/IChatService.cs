using api.DTOs.Chat;

namespace api.Interfaces
{
    public interface IChatService
    {
        Task<RoomDTO> CreateRoomAsync(string roomName, List<int>? members);
        Task<RoomDTO?> GetRoomByIdAsync(string id);
        Task<RoomDTO?> AddUserToRoomAsync(string roomId, int userId);
        Task<MessageDTO> AddMessageAsync(MessageDTO messageDTO);
        Task<RoomDTO?> AddMessageToRoomAsync(string roomId, string messageId);
        Task<RoomLessDTO?> GetRoomLessByIdAsync(string id);
    }
}
