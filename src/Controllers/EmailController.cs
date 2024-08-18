using api.DTOs.ApiResponse;
using api.DTOs.Email;
using api.Exceptions;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly ILogger<EmailController> _logger;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly IOTPService _otpService;

    public EmailController(ILogger<EmailController> logger, IEmailService emailService, ITokenService tokenService, IOTPService otpService)
    {
        _logger = logger;
        _emailService = emailService;
        _tokenService = tokenService;
        _otpService = otpService;
    }
    [HttpPost("send-verification-email")]
    public async Task<IActionResult> SendVerificationEmailAsync(VerificationRequest verificationRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new FailResponse().GetInvalidResponse());
        }
        try
        {
            var email = verificationRequest.Email;
            var token = _tokenService.GenerateVerificationToken(email);
            await _emailService.SendVerificationAsync(email, token);
            return Ok(new SuccessResponse(StatusCodes.Status200OK, $"Verification email was sent to {email}"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new FailResponse().GetInternalServerError()
            );
        }
    }
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOTPAsync(OTPRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new FailResponse().GetInvalidResponse());
        }

        try
        {
            var otp = await _otpService.GenerateOtpAsync(request.Email);
            await _emailService.SendOtpAsync(request.Email, otp);

            return Ok(new SuccessResponse(
                200,
                $"OTP was sent to your email"
            ));
        }
        catch (Exception)
        {
            throw;
        }
    }
}