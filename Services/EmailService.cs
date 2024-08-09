using System.Net;
using System.Net.Mail;
using api.Interfaces;
using api.Models;
using api.Utils;

namespace api.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationAsync(string toEmail, string token);
        Task SendOtpAsync(string toEmail, string OTP);
    }
}

namespace api.Services
{
    public class EmailService : IEmailService
    {
        private readonly String emailHost;
        private readonly int port;
        private readonly String email;
        private readonly String password;
        private readonly String apiBaseUrl;
        private readonly ITokenService _tokenService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ITokenService tokenService, ILogger<EmailService> logger)
        {
            emailHost = configuration["EmailSettings:Host"]!;
            port = int.Parse(configuration["EmailSettings:Port"]!);
            email = configuration["EmailSettings:Email"]!;
            password = configuration["EmailSettings:Password"]!;
            apiBaseUrl = configuration["ApiUrl:Base"]!;
            _tokenService = tokenService;
            _logger = logger;
        }

        private async Task SendEmailAsync(string toEmail, string subject, string message, string[]? ccEmails = null)
        {
            try
            {
                using (var smtpClient = new SmtpClient(emailHost, port)
                {
                    Credentials = new NetworkCredential(email, password),
                    EnableSsl = true,
                })
                using (var mailMessage = new MailMessage
                {
                    From = new MailAddress(email),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                })
                {
                    mailMessage.To.Add(toEmail);

                    if (ccEmails != null)
                    {
                        foreach (var ccEmail in ccEmails)
                        {
                            mailMessage.CC.Add(ccEmail);
                        }
                    }

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (SmtpFailedRecipientException ex)
            {
                // Handle recipient-related errors, like invalid email address or domain not found.
                _logger.LogError($"Failed to deliver message to {ex.FailedRecipient}. Reason:", ex.Message);
            }
            catch (SmtpException ex)
            {
                // Handle general SMTP errors
                _logger.LogError($"SMTP error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                _logger.LogError($"Unexpected error occurred: {ex.Message}");
            }
        }

        public async Task SendVerificationAsync(string toEmail, string token)
        {
            string verificationUrl = $"{apiBaseUrl}/api/auth/verify-email?token={token}";
            string verificationEmail = HtmlUtil.GetVerificationEmail(verificationUrl);

            await SendEmailAsync(toEmail, "VERIFY YOUR EMAIL", verificationEmail);
        }

        public async Task SendOtpAsync(string toEmail, string OTP)
        {
            string OTPEmail = HtmlUtil.GetOTPEmail(OTP);

            await SendEmailAsync(toEmail, "OTP", OTPEmail);
        }
    }
}
