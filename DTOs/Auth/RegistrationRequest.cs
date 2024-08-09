using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Auth;

public class RegistrationRequest
{
    [Required(ErrorMessage = "Username is required")]
    [MinLength(3, ErrorMessage = "Username is too short")]
    [MaxLength(100, ErrorMessage = "Username is too long")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    [MaxLength(100, ErrorMessage = "Email is too long")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password is >= 8 characters")]
    [MaxLength(100, ErrorMessage = "Password is too long")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Confirmed password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [MinLength(8, ErrorMessage = "Password is >= 8 characters")]
    public string ConfirmedPassword { get; set; } = "";

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [DataType(DataType.Date)]
    public DateTime? Birthdate { get; set; }
}