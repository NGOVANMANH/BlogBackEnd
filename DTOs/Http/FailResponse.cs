namespace api.DTOs.ApiResponse;

public class FailResponse : ApiResponse
{
    public Object? Errors { get; set; }
    public FailResponse(int statusCode, string message, Object? errors = null) : base(statusCode, false, message)
    {
        Errors = errors;
    }
    public FailResponse() { }
    public FailResponse GetInvalidResponse(string message = "Invalid data", Object? errors = null)
    {
        return new FailResponse(StatusCodes.Status400BadRequest, message, errors);
    }

    public FailResponse GetInternalServerError(string message = "Internal server error", Object? errors = null)
    {
        return new FailResponse(StatusCodes.Status500InternalServerError, message, errors);
    }

}