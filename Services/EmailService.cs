using System.Net;
using System.Net.Mail;
using api.Interfaces;
using api.Models;
using api.Utils;

namespace api.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailVerificationAsync(VerifyInformation verifyInformation);
    }
}

namespace api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:Host"])
            {
                Port = int.Parse(_configuration["EmailSettings:Port"]!),
                Credentials = new NetworkCredential(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:Email"]!),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendEmailVerificationAsync(VerifyInformation verifyInformation)
        {
            var host = "http://localhost:5076";
            var verificationUrl = $"{host}/api/Auth/confirm-email?VerifyToken={verifyInformation.VerifyToken}&email={verifyInformation.Email}";

            var emailBody = HtmlTemplate.GetEmailVerificationTemplate(verificationUrl);

            await SendEmailAsync(verifyInformation.Email, "Xác Thực Email", emailBody);
        }
    }
}
