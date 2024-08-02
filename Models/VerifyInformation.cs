using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class VerifyInformation
{
    [Key]
    public string Email { get; set; } = "";
    public string? VerifyToken { get; set; }
    public string? OTP { get; set; }
    public DateTime? OTPExpiry { get; set; }
    public DateTime? VerifyTokenExpiry { get; set; }
}