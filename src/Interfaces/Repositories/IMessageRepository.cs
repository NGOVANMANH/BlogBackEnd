using api.DTOs.Chat;
using api.Enities;

namespace api.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message> CreateMessageAsync(MessageDTO messageDTO);
        Task<Message> GetMessageByIdAsync(string id);
        Task<List<Message>> GetMessagesByRoomIdAsync(string roomId);
        Task<Message> UpdateMessageAsync();
        Task<Message> DeleteMessageAsync();
    }
}