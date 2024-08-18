using api.DTOs.Chat;
using api.Enities;

namespace api.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> CreateChatRoomAsync(RoomDTO chatRoomDTO);
        Task<Room?> GetChatRoomByIdAsync(string id);
        Task<Room> UpdateChatRoomAsync(string id, RoomDTO newChatRoomDTO);
        Task<Room> DeleteChatRoomAsync(string id);
        Task<Room?> AddUserToRoomAsync(string roomId, int userId);
        Task<Room?> AddMessageToRoomAsync(string roomId, string messageId);
    }
}