using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Email;

public class OTPRequest
{
    [Required(ErrorMessage = "Email property is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;
}