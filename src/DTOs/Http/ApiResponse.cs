namespace api.DTOs.ApiResponse;

public class ApiResponse
{
    public int Status { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public ApiResponse() { }
    public ApiResponse(int status, bool success, string? message)
    {
        Status = status;
        Success = success;
        Message = message;
    }
}