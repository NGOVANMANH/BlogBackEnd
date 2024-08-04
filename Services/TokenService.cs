using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.DTOs;
using api.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace api.Interfaces
{
    public interface ITokenService
    {
        public string GenerateAccessToken(UserDTO user);
        public Task<string> GenerateRefreshTokenAsync(int userId);
        public Task<UserDTO> VerifyRefreshTokenAsync(string token);
    }
}

namespace api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public TokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public string GenerateAccessToken(UserDTO user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var claims = new[]
            {
            new Claim("UserId", user.Id.ToString()),
            new Claim("Username", user.Username),
            new Claim("Email", user.Email),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
        }

        public async Task<string> GenerateRefreshTokenAsync(int userId)
        {
            var refreshToken = await _refreshTokenRepository.CreateRefreshTokenAsync(userId);
            return refreshToken.Token;
        }

        public async Task<UserDTO> VerifyRefreshTokenAsync(string token)
        {
            try
            {
                var user = await _refreshTokenRepository.VerifyRefreshTokenAsync(token);
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
    }
}