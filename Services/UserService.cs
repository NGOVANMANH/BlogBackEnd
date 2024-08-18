using api.DTOs.Auth;
using api.DTOs.User;
using api.Interfaces;
using api.Mappers;
using api.Utils;

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

namespace api.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO?> ChangePasswordByEmailAsync(string email, string newPassword)
        {
            var existingUser = await _userRepository.FindUserByEmailAsync(email);

            if (existingUser is null) return null;

            existingUser.Password = BcryptUtil.HashPassword(newPassword);

            var updateUserDTO = new UpdateUserDTO
            {
                Password = existingUser.Password,
            };

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser.Id, updateUserDTO);

            return UserMapper.ToDTO(updatedUser);
        }

        public async Task<UserDTO?> LoginUserAsync(LoginRequest login)
        {
            var user = await _userRepository.LoginUserAsync(login);
            if (user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Birthday = user.Birthday!,
                    FirstName = user.FirstName!,
                    LastName = user.LastName!
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO?> RegisterUserAsync(RegistrationRequest user)
        {
            try
            {
                var returnedUser = await _userRepository.RegisterUserAsync(user);
                return new UserDTO
                {
                    Id = returnedUser.Id,
                    Username = returnedUser.Username,
                    Email = returnedUser.Email,
                    Birthday = returnedUser.Birthday,
                    FirstName = returnedUser.FirstName!,
                    LastName = returnedUser.LastName!
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDTO> VerifyUser(string email)
        {
            try
            {
                var user = await _userRepository.VerifyUserAsync(email);

                return UserMapper.ToDTO(user)!;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
