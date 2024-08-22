using api.DTOs.Chat;
using api.Enities;
using MongoDB.Bson;

namespace api.Mappers;

public static class RoomMapper
{
    public static RoomDTO ToDTO(Room chatRoom, List<MessageDTO> messages)
    {
        return new RoomDTO
        {
            Id = chatRoom._id.ToString(),
            RoomName = chatRoom.RoomName,
            CreatedAt = chatRoom.CreatedAt,
            Members = chatRoom.Members,
            Messages = messages,
        };
    }
    public static Room ToEntity(RoomDTO chatRoomDTO)
    {
        return new Room
        {
            CreatedAt = chatRoomDTO.CreatedAt,
            RoomName = chatRoomDTO.RoomName,
            Members = chatRoomDTO.Members,
            Messages = chatRoomDTO.Messages.Select(m => ObjectId.Parse(m.Id)).ToList(),
        };
    }
    public static RoomLessDTO ToDTO(this Room room)
    {
        return new RoomLessDTO
        {
            Id = room._id.ToString(),
            CreatedAt = room.CreatedAt,
            Members = room.Members,
            Messages = room.Messages.Select(m => m.ToString()).ToList(),
            RoomName = room.RoomName,
        };
    }
}