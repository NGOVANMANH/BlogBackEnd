using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace api.Controllers
{
    [Route("ws")]
    public class WebSocketController : ControllerBase
    {
        [HttpGet("echo")]
        public async Task Get()
        {
            if (HttpContext.Items["WebSocket"] is WebSocket webSocket)
            {
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                while (!result.CloseStatus.HasValue)
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}
