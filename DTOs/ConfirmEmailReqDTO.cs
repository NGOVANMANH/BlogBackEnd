using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class ConfirmEmailReqDTO
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = "";
    [Required(ErrorMessage = "Verify token is required")]
    public string VerifyToken { get; set; } = "";
}