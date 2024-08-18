namespace api.DTOs.Chat;

public class MessageDTO
{
    public string? Id { get; set; }
    public string RoomId { get; set; } = null!;
    public int SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}