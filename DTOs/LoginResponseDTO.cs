namespace api.DTOs;

public class LoginResponseDTO
{
    public UserDTO User { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
}