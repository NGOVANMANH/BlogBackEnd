using api.Data;
using api.DTOs.Chat;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;
using api.Enities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace api.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MongoContext _mongoContext;

        public MessageRepository(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }
        public async Task<Message> CreateMessageAsync(MessageDTO messageDTO)
        {
            var message = MessageMapper.ToEntity(messageDTO);
            if (message is null)
            {
                throw new InvalidException("Object id is not valid format");
            }
            try
            {
                var newMessage = await _mongoContext.Messages.AddAsync(message);
                await _mongoContext.SaveChangesAsync();
                return newMessage.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Message> DeleteMessageAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Message> GetMessageByIdAsync(string id)
        {
            if (ObjectId.TryParse(id, out var objectId))
            {
                var existingMessage = await _mongoContext.Messages.FirstOrDefaultAsync(m => m._id == objectId);
                if (existingMessage is null)
                {
                    throw new NotFoundException("Message not found");
                }
                return existingMessage;
            }
            else
            {
                throw new InvalidException("Object id is not valid format");
            }
        }

        public async Task<List<Message>> GetMessagesByRoomIdAsync(string roomId)
        {

            if (ObjectId.TryParse(roomId, out var roomObjectId))
            {
                var messages = await _mongoContext.Messages
                    .Where(m => m.RoomId == roomObjectId)
                    .OrderByDescending(m => m.SentAt)
                    .Take(20)
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();
                return messages;
            }
            else
            {
                throw new InvalidException("Object id is not valid format");
            }
        }

        public Task<Message> UpdateMessageAsync()
        {
            throw new NotImplementedException();
        }
    }
}