using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Auth;

public class ForgotPasswordRequest
{
    [Required(ErrorMessage = "Email property is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "New password property is required")]
    [MinLength(8, ErrorMessage = "Password length is >= 8")]
    [MaxLength(100, ErrorMessage = "Password length is <= 100")]
    public string NewPassword { get; set; } = null!;
    [Required(ErrorMessage = "Confirmed pasword property is required")]
    [Compare(nameof(NewPassword), ErrorMessage = "Password does not match")]
    public string confirmedPassword { get; set; } = null!;
    [Required(ErrorMessage = "OTP property is required")]
    [MinLength(6, ErrorMessage = "OTP has 6 digits")]
    [MaxLength(6, ErrorMessage = "OTP has 6 digits")]
    public string OTP { get; set; } = null!;
}