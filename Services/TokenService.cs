using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.DTOs.User;
using api.Exceptions;
using api.Interfaces;
using Microsoft.IdentityModel.Tokens;

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
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync(int userId)
        {
            var refreshToken = await _refreshTokenRepository.CreateRefreshTokenAsync(userId);
            return refreshToken.Token;
        }

        public string GenerateVerificationToken(string userEmail)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:VerificationKey"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, userEmail),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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

        public string VerifyVerificationToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:VerificationKey"]!);

            try
            {
                // Validation parameters
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                // Validate the token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Ensure the token is a valid JWT and has not been tampered with
                if (validatedToken is JwtSecurityToken jwtToken && jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Retrieve the email from the claims
                    var emailClaim = principal.FindFirst(ClaimTypes.Email);

                    if (emailClaim != null)
                    {
                        return emailClaim.Value;
                    }
                }
            }
            catch (SecurityTokenExpiredException)
            {
                throw new TokenExpiredException();
            }
            catch (SecurityTokenException)
            {
                throw new TokenInvalidException();
            }

            throw new TokenValidateFailException();
        }

    }
}