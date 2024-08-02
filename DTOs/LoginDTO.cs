using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public String? Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(250, ErrorMessage = "Password is too long")]
    [MinLength(8, ErrorMessage = "Password is >= 8 characters")]
    public String? Password { get; set; }
}