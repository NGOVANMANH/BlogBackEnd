using api.Enities;

namespace api.Interfaces
{
    public interface IOTPService
    {
        Task<string> GenerateOtpAsync(string email);
        Task<OTPManager?> VerifyOtpAsync(string email, string OTP);
    }
}
