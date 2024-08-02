using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("/ws")]
public class WebSocketController : ControllerBase
{
    [HttpGet("chat")]
    public IActionResult Get()
    {
        return Ok();
    }
}