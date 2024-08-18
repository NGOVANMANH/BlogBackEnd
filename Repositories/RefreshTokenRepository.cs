using System.Security.Cryptography;
using api.Data;
using api.Exceptions;
using api.Interfaces;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Interfaces
{
    public interface IRefreshTokenRepository
    {
        public Task<RefreshToken> CreateRefreshTokenAsync(int userId);
        public Task<User> VerifyRefreshTokenAsync(string token);
    }
}

namespace api.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly BlogDbContext _context;

        public RefreshTokenRepository(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<RefreshToken> CreateRefreshTokenAsync(int userId)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var token = Convert.ToBase64String(randomNumber);
                var existToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);
                if (existToken != null)
                {
                    return await CreateRefreshTokenAsync(userId);
                }
                var refreshToken = new RefreshToken
                {
                    Token = token,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    Expires = DateTime.UtcNow.AddDays(7),
                    ReplacedByToken = null
                };

                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
                return refreshToken;
            }
        }

        public async Task<User> VerifyRefreshTokenAsync(string token)
        {
            var refreshToken = _context.RefreshTokens.FirstOrDefault(r => r.Token == token);
            if (refreshToken == null)
            {
                throw new TokenInvalidException();
            }
            if (refreshToken.Expires < DateTime.UtcNow)
            {
                throw new TokenExpiredException();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == refreshToken.CreatedBy);
            if (user == null)
            {
                throw new UserNotExistException();
            }
            return user;
        }
    }
}