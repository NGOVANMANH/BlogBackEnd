using MongoDB.Bson;

namespace api.Enities;

public class Message
{
    public ObjectId _id { get; set; }
    public ObjectId RoomId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
