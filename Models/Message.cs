using MongoDB.Bson;

namespace api.MongoDB.Models;

public class Message
{
    public ObjectId _id { get; set; }
    public string content { get; set; } = null!;
    public DateTime timestamp { get; set; }
    public int userID { get; set; }
}