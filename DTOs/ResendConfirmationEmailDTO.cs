using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class ResendConfirmationEmailDTO
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = "";
}