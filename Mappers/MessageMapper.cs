using api.DTOs.Chat;
using api.Enities;
using MongoDB.Bson;

namespace api.Mappers;

public static class MessageMapper
{
    public static MessageDTO ToDTO(Message message)
    {
        return new MessageDTO
        {
            Id = message._id.ToString(),
            Content = message.Content,
            RoomId = message.RoomId.ToString(),
            SenderId = message.SenderId,
            SentAt = message.SentAt,
        };
    }
    public static MessageDTO ToDTO(MessageRequest messageRequest, int senderId)
    {
        return new MessageDTO
        {
            Content = messageRequest.Content,
            RoomId = messageRequest.RoomId,
            SenderId = senderId,
            SentAt = DateTime.UtcNow,
        };
    }
    public static Message? ToEntity(MessageDTO messageDTO)
    {
        if (ObjectId.TryParse(messageDTO.RoomId, out var objectId))
        {
            return new Message
            {
                RoomId = objectId,
                Content = messageDTO.Content,
                SenderId = messageDTO.SenderId,
                SentAt = messageDTO.SentAt,
            };
        }
        else
        {
            return null;
        }
    }
}