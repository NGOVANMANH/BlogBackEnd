using api.DTOs.Auth;
using api.DTOs.User;
using api.Entities;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<User> LoginUserAsync(LoginRequest loginRequest);
        Task<User> RegisterUserAsync(RegistrationRequest registrationRequest);
        Task<User> VerifyUserAsync(string email);
        Task<User> UpdateUserAsync(int id, UpdateUserDTO updateUserDTO);
        Task<User> CreateUserAsync(UserDTO user);
        Task<User?> FindUserByIdAsync(int id);
        Task<User?> FindUserByEmailAsync(string email);
        Task<User?> FindUserByUsernameAsync(string username);
    }
}