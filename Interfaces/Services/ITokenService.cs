using api.DTOs.User;

namespace api.Interfaces
{
    public interface ITokenService
    {
        public string GenerateAccessToken(UserDTO user);
        public Task<string> GenerateRefreshTokenAsync(int userId);
        public Task<UserDTO> VerifyRefreshTokenAsync(string token);
        public string GenerateVerificationToken(string userEmail);
        public string VerifyVerificationToken(string token);
    }
}