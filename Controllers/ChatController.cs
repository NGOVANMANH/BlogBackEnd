using System.Net.WebSockets;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("ws/[controller]")]
public class ChatController : ControllerBase
{
    private readonly WsHandler _webSocketHandler;

    public ChatController(WsHandler webSocketHandler)
    {
        _webSocketHandler = webSocketHandler;
    }

    [HttpGet]
    // [Authorize]
    public async Task Get()
    {
        if (HttpContext.Items["WebSocket"] is WebSocket webSocket)
        {
            var socketId = Guid.NewGuid().ToString();
            _webSocketHandler.AddSocket(socketId, webSocket);

            await _webSocketHandler.ReceiveMessageAsync(socketId);
        }
        else
        {
            HttpContext.Response.StatusCode = 400;
        }
    }
}