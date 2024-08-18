using MongoDB.Bson;

namespace api.Enities;

public class Room
{
    public ObjectId _id { get; set; }
    public string RoomName { get; set; } = null!;
    public List<int> Members { get; set; } = new List<int>();
    public List<ObjectId> Messages { get; set; } = new List<ObjectId>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}