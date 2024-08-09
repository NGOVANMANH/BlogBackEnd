using System.Net.WebSockets;
using System.Security.Claims;
using api.Data;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("ws/[controller]")]
public class ChatController : ControllerBase
{
    private readonly WsHandler _webSocketHandler;
    private readonly MongoContext _mogoContext;

    public ChatController(WsHandler webSocketHandler, MongoContext mogoContext)
    {
        _webSocketHandler = webSocketHandler;
        _mogoContext = mogoContext;
    }

    [HttpGet]
    // [Authorize]
    public async Task Get()
    {
        if (HttpContext.Items["WebSocket"] is WebSocket webSocket)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            if (userId == null)
            {
                HttpContext.Response.StatusCode = 401;
            }

            var userIdToInt = int.Parse(userId!);

            await _webSocketHandler.AddSocketAsync(userIdToInt, webSocket);

            await _webSocketHandler.ReceiveMessageAsync(userIdToInt);
        }
        else
        {
            HttpContext.Response.StatusCode = 400;
        }
    }
}