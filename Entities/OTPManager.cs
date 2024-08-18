using MongoDB.Bson;

namespace api.Enities;

public class OTPManager
{
    public ObjectId _id { get; set; }
    public string Email { get; set; } = null!;
    public string OTP { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
}