using api.DTOs.Chat;
using api.Interfaces;
using api.Mappers;

namespace api.Interfaces
{
    public interface IChatService
    {
        Task<RoomDTO> CreateRoomAsync(string roomName, List<int>? members);
        Task<RoomDTO?> GetRoomByIdAsync(string id);
        Task<RoomDTO?> AddUserToRoomAsync(string roomId, int userId);
        Task<MessageDTO> AddMessageAsync(MessageDTO messageDTO);
        Task<RoomDTO?> AddMessageToRoomAsync(string roomId, string messageId);
    }
}

namespace api.Services
{
    public class ChatService : IChatService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMessageRepository _messageRepository;

        public ChatService(IRoomRepository roomRepository, IMessageRepository messageRepository)
        {
            _roomRepository = roomRepository;
            _messageRepository = messageRepository;
        }

        public async Task<MessageDTO> AddMessageAsync(MessageDTO messageDTO)
        {
            var message = await _messageRepository.CreateMessageAsync(messageDTO);
            return MessageMapper.ToDTO(message);
        }

        public async Task<RoomDTO?> AddMessageToRoomAsync(string roomId, string messageId)
        {
            var room = await _roomRepository.AddMessageToRoomAsync(roomId, messageId);
            if (room is null)
            {
                return null;
            }
            var messages = await _messageRepository.GetMessagesByRoomIdAsync(room._id.ToString());
            var messagesDTO = messages.Select(m => Mappers.MessageMapper.ToDTO(m)).ToList();
            return Mappers.RoomMapper.ToDTO(room, messagesDTO);
        }

        public async Task<RoomDTO?> AddUserToRoomAsync(string roomId, int userId)
        {
            var room = await _roomRepository.AddUserToRoomAsync(roomId, userId);
            if (room is null) return null;
            var messages = await _messageRepository.GetMessagesByRoomIdAsync(room._id.ToString());
            var messagesDTO = messages.Select(m => Mappers.MessageMapper.ToDTO(m)).ToList();
            return Mappers.RoomMapper.ToDTO(room, messagesDTO);
        }

        public async Task<RoomDTO> CreateRoomAsync(string roomName, List<int>? members)
        {
            var roomDTO = new RoomDTO
            {
                RoomName = roomName,
                CreatedAt = DateTime.UtcNow,
                Members = members is null ? new List<int>() : members,
            };
            var room = await _roomRepository.CreateChatRoomAsync(roomDTO);

            return RoomMapper.ToDTO(room, new List<MessageDTO>());
        }

        public async Task<RoomDTO?> GetRoomByIdAsync(string id)
        {
            var room = await _roomRepository.GetChatRoomByIdAsync(id);
            if (room is null) return null;
            var messages = await _messageRepository.GetMessagesByRoomIdAsync(room._id.ToString());
            var messagesDTO = messages.Select(m => Mappers.MessageMapper.ToDTO(m)).ToList();
            return Mappers.RoomMapper.ToDTO(room, messagesDTO);
        }
    }
}