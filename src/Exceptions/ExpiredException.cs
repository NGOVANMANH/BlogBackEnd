namespace api.Exceptions;

public class ExpiredException : Exception
{
    public int? ErrorCode { get; set; }
    public ExpiredException(string message = "Value Expired", int? errorCode = null) : base(message)
    { }
}