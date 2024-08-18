namespace api.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationAsync(string toEmail, string token);
        Task SendOtpAsync(string toEmail, string OTP);
    }
}