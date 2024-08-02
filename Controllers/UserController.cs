using api.DTOs;
using api.Interfaces;
using api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public UserController(IUserService userService, ITokenService tokenService, IEmailService emailService)
    {
        _userService = userService;
        _tokenService = tokenService;
        _emailService = emailService;
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
            var user = await _userService.RegisterUserAsync(registerDTO);
            return Created(
                "api/users/register",
                new ApiResponseDTO(
                    true,
                    "User registered successfully",
                    user
                    )
            );
        }
        catch (Exception e)
        {
            return BadRequest(
                new ApiResponseDTO(
                    false,
                    e.Message
                    ));
        }
    }

    // [HttpPost("login")]
    // public async Task<IActionResult> LoginUserAsync(LoginDTO loginDTO)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(
    //             new ApiResponseDTO(
    //                 false,
    //                 "Invalid data",
    //                 new
    //                 {
    //                     errors = ModelStateUtil.FormatModelStateErrors(ModelState)
    //                 }
    //                 ));
    //     }
    //     var user = await _userService.LoginUserAsync(loginDTO);
    //     if (user == null)
    //     {
    //         return Unauthorized(
    //             new ApiResponseDTO(
    //                 false,
    //                 "Invalid credentials"
    //                 ));
    //     }

    //     var accessToken = _tokenService.GenerateAccessToken(user);
    //     var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

    //     Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
    //     {
    //         HttpOnly = true,
    //         SameSite = SameSiteMode.None,
    //         Secure = false,
    //         Expires = DateTime.UtcNow.AddDays(7)
    //     });

    //     return Ok(new ApiResponseDTO(
    //         true,
    //         "User logged in successfully",
    //         new LoginResponseDTO
    //         {
    //             User = user,
    //             AccessToken = accessToken
    //         }
    //         ));
    // }

    // [HttpGet("2fa")]
    // public IActionResult GetTwoFactorAuthenticationAsync()
    // {
    //     var secret = _userService.GenerateTwoFactorAuthenticationSecret();
    //     var qrCode = _userService.GenerateTwoFactorAuthenticationQRCode(secret);

    //     return Ok(new ApiResponseDTO(
    //         true,
    //         "Two-factor authentication setup successful",
    //         new
    //         {
    //             secret,
    //             qrCode
    //         }
    //         ));
    // }

    // [HttpGet("profile/{id}")]
    // [Authorize]
    // public IActionResult GetProfieAsync([FromRoute] int id)
    // {
    //     return Ok(new ApiResponseDTO(
    //         true,
    //         "User profile retrieved successfully",
    //         new
    //         {
    //             id,
    //         }
    //         ));
    // }
}