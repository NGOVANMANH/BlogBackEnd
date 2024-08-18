namespace api.Exceptions;

public class InvalidException : Exception
{
    public int? ErrorCode { get; set; }
    public InvalidException(string message = "In valid data", int? errorCode = null) : base(message)
    {
        ErrorCode = errorCode;
    }
}
