using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class ForgotPasswordDTO
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "OTP is required")]
    [MinLength(6, ErrorMessage = "OTP must be at least 6 characters long")]
    [MaxLength(6, ErrorMessage = "OTP must be at most 6 characters long")]
    public string OTP { get; set; } = string.Empty;
}