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
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
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
            await _authService.LoginUserAsync(loginDTO);
            return Ok(new ApiResponseDTO(true, "Login successful"));
        }
        catch (System.Exception)
        {

            throw;
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