using api.DTOs;
using api.Exceptions;
using api.Interfaces;
using api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegisterDTO registerDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new ApiResponseDTO(
                    false,
                    "Invalid data",
                    new
                    {
                        errors = ModelStateUtil.FormatModelStateErrors(ModelState)
                    }
                ));
        }
        try
        {
            await _authService.RegisterUserAsync(registerDTO);
            return Created(
                "api/auth/register",
                new ApiResponseDTO(
                    true,
                    "Please verrify your email"));
        }
        catch (UserAlreadyExistException e)
        {
            return BadRequest(new ApiResponseDTO(false, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new ApiResponseDTO(false, "An error occurred while registering the user"));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync(LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new ApiResponseDTO(
                    false,
                    "Invalid data",
                    new { errors = ModelStateUtil.FormatModelStateErrors(ModelState) }));
        }

        try
        {
            var user = await _authService.LoginUserAsync(loginDTO);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = false,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new ApiResponseDTO(true, "Login successful", new { user, accessToken }));
        }
        catch (UserNotExistException e)
        {
            return BadRequest(new ApiResponseDTO(false, "Email or password is incorrect"));
        }
        catch (UserNotVerifiedException e)
        {
            return BadRequest(new ApiResponseDTO(false, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new ApiResponseDTO(false, "An error occurred while logging in"));
        }
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailReqDTO confirmEmailReqDTO)
    {
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ApiResponseDTO(
                        false,
                        "Invalid data",
                        new { errors = ModelStateUtil.FormatModelStateErrors(ModelState) }));
            }

            confirmEmailReqDTO.VerifyToken = confirmEmailReqDTO.VerifyToken.Replace(" ", "+");

            try
            {
                await _authService.ConfirmEmailAsync(new EmailConfirmationDTO
                {
                    Email = confirmEmailReqDTO.Email,
                    VerifyToken = confirmEmailReqDTO.VerifyToken
                });

                var htmlContent = HtmlTemplate.ThanksForConfirmingEmail;
                return Content(htmlContent, "text/html");
            }
            catch (TokenInvalidException e)
            {
                return BadRequest(new ApiResponseDTO(false, e.Message));
            }
            catch (TokenExpiredException e)
            {
                return BadRequest(new ApiResponseDTO(false, e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, new ApiResponseDTO(false, "An error occurred while confirming the email"));
            }
        }
    }

    [HttpPost("resend-confirm-email")]
    public async Task<IActionResult> ResendConfirmationEmailAsync(ResendConfirmationEmailDTO resendConfirmationEmailDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new ApiResponseDTO(
                    false,
                    "Invalid data",
                    new { errors = ModelStateUtil.FormatModelStateErrors(ModelState) }));
        }

        try
        {
            await _authService.ResendConfirmationEmailAsync(resendConfirmationEmailDTO);
            return Ok(new ApiResponseDTO(true, "Confirmation email has been sent"));
        }
        catch (UserNotExistException e)
        {
            return BadRequest(new ApiResponseDTO(false, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new ApiResponseDTO(false, "An error occurred while resending the confirmation email"));
        }
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new ApiResponseDTO(false, "Refresh token is required"));
            }

            var user = await _tokenService.VerifyRefreshTokenAsync(refreshToken);
            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);
            var accessToken = _tokenService.GenerateAccessToken(user);

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = false,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new ApiResponseDTO(true, "Token refreshed", new { accessToken }));
        }
        catch (TokenExpiredException e)
        {
            return BadRequest(new ApiResponseDTO(false, e.Message));
        }
        catch (TokenInvalidException e)
        {
            return BadRequest(new ApiResponseDTO(false, e.Message));
        }
        catch (UserNotExistException e)
        {
            return BadRequest(new ApiResponseDTO(false, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new ApiResponseDTO(false, "An error occurred while refreshing the token"));
        }
    }
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("refreshToken");
        return Ok(new ApiResponseDTO(true, "Logout successful"));
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOTPAsync(OTPRequestDTO oTPRequestDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new ApiResponseDTO(
                    false,
                    "Invalid data",
                    new { errors = ModelStateUtil.FormatModelStateErrors(ModelState) }));
        }
        try
        {
            await _authService.SendOTPAsync(oTPRequestDTO);
            return Ok(new ApiResponseDTO(true, "OTP has been sent to your email"));
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDTO(false, e.Message));
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new ApiResponseDTO(
                    false,
                    "Invalid data",
                    new { errors = ModelStateUtil.FormatModelStateErrors(ModelState) }));
        }

        try
        {
            await _authService.ForgotPasswordAsync(forgotPasswordDTO);
            return Ok(new ApiResponseDTO(true, "Reset password successful"));
        }
        catch (UserNotExistException)
        {
            return BadRequest(new ApiResponseDTO(false, "Email does not exist"));
        }
        catch (TokenExpiredException)
        {
            return BadRequest(new ApiResponseDTO(false, "OTP has expired"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new ApiResponseDTO(false, "An error occurred while reset password"));
        }
    }
}