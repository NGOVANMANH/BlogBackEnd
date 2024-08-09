using System.Text.Json;
using api.DTOs;
using api.DTOs.ApiResponse;

namespace api.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    public WebSocketMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/ws"))
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                await _next(context);
                var ws = await context.WebSockets.AcceptWebSocketAsync();
                context.Items["WebSocket"] = ws;
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new FailResponse(40, "Hmm...");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
        else
        {
            await _next(context);
        }
    }
}