using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Chat;

public class RoomRequest
{
    [Required]
    [MaxLength(100)]
    [MinLength(2)]
    public string RoomName { get; set; } = null!;
    public List<int>? MemberIds { get; set; }
}