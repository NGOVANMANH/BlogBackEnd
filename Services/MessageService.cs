using api.Data;
using api.DTOs;
using api.Interfaces;
using api.MongoDB.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesByUserIdAsync();
        Task<Message> SaveMessageAsync(ChatMessageDTO chatMessageDTO);
    }
}

namespace api.Services
{
    public class MessageService : IMessageService
    {
        private readonly MongoContext _context;
        public MessageService(MongoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Message>> GetMessagesByUserIdAsync()
        {
            var messages = await _context.Messages.ToListAsync();
            if (messages is null)
            {
                return new List<Message>();
            }
            return messages;
        }

        public async Task<Message> SaveMessageAsync(ChatMessageDTO chatMessageDTO)
        {
            var newMessages = await _context.Messages.AddAsync(new Message
            {
                content = chatMessageDTO.Content,
                userID = chatMessageDTO.UserId,
                timestamp = chatMessageDTO.Timestamp
            });

            await _context.SaveChangesAsync();

            return newMessages.Entity;
        }
    }
}