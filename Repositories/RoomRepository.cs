using api.Data;
using api.DTOs.Chat;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;
using api.Enities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

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

namespace api.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MongoContext _mongoContext;

        public RoomRepository(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<Room?> AddMessageToRoomAsync(string roomId, string messageId)
        {

            var room = await GetChatRoomByIdAsync(roomId);
            if (room is null) return null;
            if (ObjectId.TryParse(messageId, out var objectId))
            {
                room.Messages.Add(objectId);
                await _mongoContext.SaveChangesAsync();
                return room;
            }
            return null;
        }

        public async Task<Room?> AddUserToRoomAsync(string roomId, int userId)
        {
            var room = await GetChatRoomByIdAsync(roomId);
            if (room is null) return null;
            room.Members.Add(userId);
            await _mongoContext.SaveChangesAsync();
            return room;
        }

        public async Task<Room> CreateChatRoomAsync(RoomDTO chatRoomDTO)
        {
            var chatRoom = RoomMapper.ToEntity(chatRoomDTO);
            var newChatRoom = await _mongoContext.Rooms.AddAsync(chatRoom);
            await _mongoContext.SaveChangesAsync();
            return newChatRoom.Entity;
        }

        public Task<Room> DeleteChatRoomAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Room?> GetChatRoomByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }
            var existingChatRoom = await _mongoContext.Rooms.FirstOrDefaultAsync(c => c._id == objectId);
            return existingChatRoom;
        }

        public Task<Room> UpdateChatRoomAsync(string id, RoomDTO newChatRoomDTO)
        {
            throw new NotImplementedException();
        }
    }
}