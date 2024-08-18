using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Chat;

public class MessageRequest
{
    [Required]
    public string RoomId { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
}