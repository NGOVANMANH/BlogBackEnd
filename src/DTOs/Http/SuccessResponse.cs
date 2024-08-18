using api.DTOs.User;

namespace api.DTOs.ApiResponse;

public class SuccessResponse : ApiResponse
{
    public Object? Data { get; set; }
    public SuccessResponse(int statusCode, string? message, Object? data = null) : base(statusCode, true, message)
    {
        Data = data;
    }
    public SuccessResponse() { }
}