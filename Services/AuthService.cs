using System.Buffers.Text;
using System.Security.Cryptography;
using api.DTOs;
using api.Interfaces;
using api.Models;

namespace api.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterDTO registerDTO);
        Task<UserDTO> LoginUserAsync(LoginDTO loginDTO);
        Task<Boolean> ConfirmEmailAsync(EmailConfirmationDTO emailConfirmationDTO);
    }
}

namespace api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IVerifyInformationRepository _verifyInformationRepository;

        public AuthService(IUserRepository userRepository, IEmailService emailService, IVerifyInformationRepository verifyInformationRepository)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _verifyInformationRepository = verifyInformationRepository;
        }
        private string GenerateVerifyToken()
        {
            byte[] randomBytes = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            string randomBase64String = Convert.ToBase64String(randomBytes);

            return randomBase64String;
        }
        public async Task RegisterUserAsync(RegisterDTO registerDTO)
        {
            try
            {
                var newUser = await _userRepository.RegisterUserAsync(registerDTO);
                await _emailService.SendEmailVerificationAsync(
                    await _verifyInformationRepository
                        .CreateVerifyInformationAsync(
                            new VerifyInformation
                            {
                                Email = newUser.Email,
                                VerifyToken = GenerateVerifyToken(),
                                VerifyTokenExpiry = DateTime.UtcNow.AddHours(1)
                            }));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDTO> LoginUserAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userRepository.LoginUserAsync(loginDTO);
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Boolean> ConfirmEmailAsync(EmailConfirmationDTO emailConfirmationDTO)
        {
            try
            {
                var IsVerified = await _verifyInformationRepository.VerifyInformationAsync(emailConfirmationDTO);
                if (IsVerified)
                {
                    await _userRepository.SetUserVerificationAsync(emailConfirmationDTO.Email, true);
                }
                return IsVerified;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}