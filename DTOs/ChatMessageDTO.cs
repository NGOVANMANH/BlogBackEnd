namespace api.DTOs;
public class ChatMessageDTO
{
    public string Sender { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}