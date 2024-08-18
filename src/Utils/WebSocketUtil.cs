using System.Text;
using System.Text.Json;

namespace api.Utils
{
    public static class WebSocketUtil
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Handle special characters correctly
        };

        public static ArraySegment<byte> CreateMessageSegment<T>(T message)
        {
            var messageJson = JsonSerializer.Serialize(message, JsonOptions);
            var messageBuffer = Encoding.UTF8.GetBytes(messageJson);
            return new ArraySegment<byte>(messageBuffer);
        }

        public static T? ParseMessage<T>(ArraySegment<byte> buffer)
        {
            var messageJson = Encoding.UTF8.GetString(buffer.Array!, buffer.Offset, buffer.Count);
            return JsonSerializer.Deserialize<T>(messageJson, JsonOptions);
        }
    }
}
