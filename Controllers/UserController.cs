using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // [HttpGet("profile")]
    // [Authorize]
    // public async Task<IActionResult> GetUserProfileAsync()
    // {
    //     return Ok();
    // }
}