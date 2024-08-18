using api.DTOs.User;
using api.DTOs.Auth;
using api.Interfaces;
using api.Mappers;

namespace api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> RegisterUserAsync(RegistrationRequest registrationRequest)
        {
            try
            {
                var newUser = await _userRepository.RegisterUserAsync(registrationRequest);

                return UserMapper.ToDTO(newUser)!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDTO> LoginUserAsync(LoginRequest loginRequest)
        {
            try
            {
                var user = await _userRepository.LoginUserAsync(loginRequest);
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Birthday = user.Birthday,
                    FirstName = user.FirstName!,
                    LastName = user.LastName!
                };

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}