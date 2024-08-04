using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class OTPRequestDTO
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
}