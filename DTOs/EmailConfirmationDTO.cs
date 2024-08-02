namespace api.DTOs;

public class EmailConfirmationDTO
{
    public string Email { get; set; } = null!;
    public string VerifyToken { get; set; } = null!;
}