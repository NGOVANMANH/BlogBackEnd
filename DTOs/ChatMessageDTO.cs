using MongoDB.Bson;

namespace api.DTOs;
public class ChatMessageDTO
{
    public int UserId;
    public string Content { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}