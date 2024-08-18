using api.DTOs.Auth;
using api.DTOs.User;

namespace api.Interfaces
{
    public interface IAuthService
    {
        Task<UserDTO> RegisterUserAsync(RegistrationRequest registrationRequest);
        Task<UserDTO> LoginUserAsync(LoginRequest loginRequest);
    }
}