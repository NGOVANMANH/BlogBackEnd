using api.Enities;

namespace api.Interfaces
{
    public interface IOTPRepository
    {
        Task<OTPManager> CreateOtpAsync(OTPManager otpManager);
        Task<OTPManager> FindOtpByEmailAsync(string email);
        Task<OTPManager> UpdateOtpAsync(OTPManager otpManager);
    }
}
