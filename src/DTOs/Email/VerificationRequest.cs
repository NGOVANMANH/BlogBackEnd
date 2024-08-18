using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Email;

public class VerificationRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;
}