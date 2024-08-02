using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class EmailDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Subject { get; set; } = null!;
    [Required]
    public string Body { get; set; } = null!;
}