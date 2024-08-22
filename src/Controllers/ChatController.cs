using System.Net.WebSockets;
using System.Security.Claims;
using api.DTOs.ApiResponse;
using api.DTOs.Chat;
using api.Extensions.ModelState;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IWsChatService _wsChatService;
    private readonly IChatService _chatService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(ILogger<ChatController> logger, IWsChatService wsChatService, IChatService chatService)
    {
        _wsChatService = wsChatService;
        _chatService = chatService;
        _logger = logger;
    }

    [HttpGet("/ws")]
    [Authorize]
    public async Task<IActionResult> WebSocketEndpoint()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            WebSocket? webSocket = null;
            webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                if (userId == null)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "User not authenticated", CancellationToken.None);
                    return Unauthorized(new FailResponse(401, "User not authenticated"));
                }

                var intUserId = int.Parse(userId);
                _wsChatService.AddConnection(intUserId, webSocket);
                await _wsChatService.HandleWebSocketCommunicationAsync(intUserId);
            }
            catch (Exception e)
            {
                // Log the exception since we can't modify the response headers
                _logger.LogError(e, "An error occurred while handling the WebSocket connection.");

                if (webSocket != null && webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal Server Error", CancellationToken.None);
                    _wsChatService.RemoveConnection(int.Parse(userId!));
                }
            }
            finally
            {
                // Ensure the WebSocket is closed if it's still open
                if (webSocket != null && webSocket.State != WebSocketState.Closed)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the server", CancellationToken.None);
                    _wsChatService.RemoveConnection(int.Parse(userId!));
                }
            }

            return new EmptyResult(); // Response is already started, returning EmptyResult
        }
        else
        {
            return BadRequest(new FailResponse(400, "WebSocket connection expected"));
        }
    }

    [HttpPost("room")]
    [Authorize]
    public async Task<IActionResult> CreateRoomAsync(RoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new FailResponse().GetInvalidResponse(error: ModelState.GetErrors()));
        }
        try
        {
            var room = await _chatService.CreateRoomAsync(request.RoomName, request.MemberIds);

            return Created("", new SuccessResponse(201, "Create room successful", room));
        }
        catch (Exception)
        {
            return StatusCode(500, new FailResponse().GetInternalServerError());
        }
    }
    [HttpGet("room/{id}")]
    [Authorize]
    public async Task<IActionResult> GetRoomAsync(string id, [FromQuery] string? type = null)
    {
        const string MESSAGE_ID_ONLY = "less";

        if (id is null || !ObjectId.TryParse(id, out _))
        {
            return BadRequest(new FailResponse().GetInvalidResponse(error: new
            {
                id = "Id invalid."
            }));
        }
        if (type == MESSAGE_ID_ONLY)
        {
            var room = await _chatService.GetRoomLessByIdAsync(id);
            return Ok(new SuccessResponse(200, "Get room successfully.", room));
        }
        else
        {
            var room = await _chatService.GetRoomByIdAsync(id);
            return Ok(new SuccessResponse(200, "Get room successfully.", room));
        }
    }
}