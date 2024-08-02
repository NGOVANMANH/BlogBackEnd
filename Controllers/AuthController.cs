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
            return BadRequest(new ApiResponseDTO(false, e.Message));
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
            catch (VerifyTokenInvalidException e)
            {
                return BadRequest(new ApiResponseDTO(false, e.Message));
            }
            catch (VerifyTokenExpiredException e)
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
}