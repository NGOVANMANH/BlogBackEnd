using api.Entities;

namespace api.Interfaces
{
    public interface IRefreshTokenRepository
    {
        public Task<RefreshToken> CreateRefreshTokenAsync(int userId);
        public Task<User> VerifyRefreshTokenAsync(string token);
    }
}