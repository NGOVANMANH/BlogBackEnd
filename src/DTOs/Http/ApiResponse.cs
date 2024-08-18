namespace api.DTOs.ApiResponse;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public ApiResponse() { }
    public ApiResponse(int statusCode, bool success, string? message)
    {
        StatusCode = statusCode;
        Success = success;
        Message = message;
    }
}