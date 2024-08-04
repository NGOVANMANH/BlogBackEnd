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
        Task ResendConfirmationEmailAsync(ResendConfirmationEmailDTO resendConfirmationEmailDTO);
        Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO);
        Task SendOTPAsync(OTPRequestDTO otpRequestDTO);
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
                    FirstName = user.FirstName!,
                    LastName = user.LastName!
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

        public async Task ResendConfirmationEmailAsync(ResendConfirmationEmailDTO resendConfirmationEmailDTO)
        {
            try
            {
                await _emailService.SendEmailVerificationAsync(
                    await _verifyInformationRepository
                        .CreateVerifyInformationAsync(
                            new VerifyInformation
                            {
                                Email = resendConfirmationEmailDTO.Email,
                                VerifyToken = GenerateVerifyToken(),
                                VerifyTokenExpiry = DateTime.UtcNow.AddHours(1)
                            }));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendOTPAsync(OTPRequestDTO otpRequestDTO)
        {
            try
            {
                var verifyInformation = await _verifyInformationRepository.CreateVerifyInformationAsync(
                    new VerifyInformation
                    {
                        Email = otpRequestDTO.Email,
                        OTP = new Random().Next(100000, 999999).ToString(),
                        OTPExpiry = DateTime.UtcNow.AddMinutes(5)
                    });

                await _emailService.SendOTPAsync(verifyInformation);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
                var user = await _verifyInformationRepository.VerifyOTPAsync(forgotPasswordDTO);
                user.Password = forgotPasswordDTO.NewPassword;
                await _userRepository.UpdateUserAsync(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}