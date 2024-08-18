namespace api.DTOs.ApiResponse;

public class FailResponse : ApiResponse
{
    public Object? Error { get; set; }
    public FailResponse(int statusCode, string? message, Object? error = null) : base(statusCode, false, message)
    {
        Error = error;
    }
    public FailResponse() { }
    public FailResponse GetInvalidResponse(string message = "Invalid data", Object? error = null)
    {
        return new FailResponse(StatusCodes.Status400BadRequest, message, error);
    }

    public FailResponse GetInternalServerError(string message = "Internal server error", Object? error = null)
    {
        return new FailResponse(StatusCodes.Status500InternalServerError, message, error);
    }

}