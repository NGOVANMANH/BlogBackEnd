using api.DTOs.Auth;
using api.DTOs.User;

namespace api.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO?> RegisterUserAsync(RegistrationRequest user);
        Task<UserDTO?> LoginUserAsync(LoginRequest login);
        Task<UserDTO> VerifyUser(string email);
        Task<UserDTO?> ChangePasswordByEmailAsync(string email, string newPassword);
    }
}