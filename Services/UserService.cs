using api.DTOs;
using api.Interfaces;

namespace api.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO?> RegisterUserAsync(RegisterDTO user);
        Task<UserDTO?> LoginUserAsync(LoginDTO login);
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

        public async Task<UserDTO?> LoginUserAsync(LoginDTO login)
        {
            var user = await _userRepository.LoginUserAsync(login);
            if (user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Birthdate = user.Birthdate,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO?> RegisterUserAsync(RegisterDTO user)
        {
            try
            {
                var returnedUser = await _userRepository.RegisterUserAsync(user);
                return new UserDTO
                {
                    Id = returnedUser.Id,
                    Username = returnedUser.Username,
                    Email = returnedUser.Email,
                    Birthdate = returnedUser.Birthdate,
                    FirstName = returnedUser.FirstName,
                    LastName = returnedUser.LastName
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
