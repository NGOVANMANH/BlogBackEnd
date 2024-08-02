using System.Security.Cryptography;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Interfaces
{
    public interface IRefreshTokenRepository
    {
        public Task<RefreshToken> CreateRefreshTokenAsync(int userId);
        public Task<RefreshToken> GetRefreshTokenByTokenAsync(string token);
        public Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken refreshToken);
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
                    ReplacedByToken = null,
                    Revoked = DateTime.UtcNow.AddDays(7)
                };

                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
                return refreshToken;
            }
        }

        public Task<RefreshToken> GetRefreshTokenByTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}