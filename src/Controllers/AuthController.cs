using api.DTOs;
using api.DTOs.ApiResponse;
using api.DTOs.Auth;
using api.Exceptions;
using api.Extensions.ModelState;
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
    private readonly IUserService _userService;
    private readonly IOTPService _oTPService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ITokenService tokenService,
        IUserService userService,
        IOTPService oTPService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _tokenService = tokenService;
        _userService = userService;
        _oTPService = oTPService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegistrationRequest registrationRequest)
    {
        if (!ModelState.IsValid)
        {
            // Log error 
            var errors = ModelState.GetErrors();
            foreach (var error in errors)
            {
                _logger.LogError("Model validation failed: {Key} - {Error}", error.Key, error.Value);
            }

            // return response
            return BadRequest(new FailResponse().GetInvalidResponse(error: errors));
        }

        try
        {
            // call service handle register
            var newUser = await _authService.RegisterUserAsync(registrationRequest);

            // return response
            return Created(
                "api/auth/register",
                new SuccessResponse(StatusCodes.Status201Created,
                "Register successfull, please verify email to use this account",
                new
                {
                    user = newUser
                }));
        }
        catch (AlreadyExistException)
        {
            return BadRequest(new FailResponse(StatusCodes.Status400BadRequest, "Email or username is already exist"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
            new FailResponse().GetInternalServerError());
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new FailResponse(400, "Invalid Data", ModelState.GetErrors()));
        }

        try
        {
            var user = await _authService.LoginUserAsync(loginRequest);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new SuccessResponse(
                StatusCodes.Status200OK,
                "Login successfull",
                new
                {
                    user,
                    token = accessToken
                }));
        }
        catch (UserNotExistException)
        {
            return NotFound(new FailResponse(404, "Email or password is incorrect"));
        }
        catch (UserNotVerifiedException e)
        {
            return BadRequest(new FailResponse(400, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new FailResponse().GetInternalServerError());
        }
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new FailResponse(400, "Refresh token is required"));
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

            return Ok(new SuccessResponse(200, "Token refreshed", new { token = accessToken }));
        }
        catch (TokenExpiredException e)
        {
            return BadRequest(new FailResponse(400, e.Message));
        }
        catch (TokenInvalidException e)
        {
            return BadRequest(new FailResponse(400, e.Message));
        }
        catch (UserNotExistException e)
        {
            return BadRequest(new FailResponse(400, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, new FailResponse().GetInternalServerError());
        }
    }
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("refreshToken");
        return Ok(new SuccessResponse(200, "Logout successfull"));
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        if (token is null)
        {
            return Content(HtmlUtil.GetVerificationResultPage("invalid"), "text/html");
        }

        try
        {
            var email = _tokenService.VerifyVerificationToken(token);
            var user = await _userService.VerifyUser(email);

            return Content(HtmlUtil.GetVerificationResultPage("success"), "text/html");
        }
        catch (NotFoundException)
        {
            return Content(HtmlUtil.GetVerificationResultPage("notfound"), "text/html");
        }
        catch (Exception)
        {
            return Content(HtmlUtil.GetVerificationResultPage(), "text/html");
        }
    }
    [HttpPatch("forgot-password")]
    public async Task<IActionResult> ForgetPassword(ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new FailResponse().GetInvalidResponse(error: ModelState.GetErrors()));
        }
        try
        {

            var otp = await _oTPService.VerifyOtpAsync(request.Email, request.OTP);

            if (otp is null)
            {
                return BadRequest(new FailResponse(400, "OTP does not match"));
            }

            var updatedUser = await _userService.ChangePasswordByEmailAsync(request.Email, request.NewPassword);

            if (updatedUser is null)
            {
                return Unauthorized(new FailResponse(401, "The email has not been registered before"));
            }

            return Ok(new SuccessResponse(200, "Password reset successfully"));
        }
        catch (ExpiredException)
        {
            return BadRequest(new FailResponse(400, "OTP is expired, please resend otp"));
        }
        catch (Exception)
        {
            return StatusCode(500, new FailResponse().GetInternalServerError());
        }
    }
}