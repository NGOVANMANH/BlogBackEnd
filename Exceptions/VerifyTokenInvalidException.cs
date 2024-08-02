namespace api.Exceptions;

public class VerifyTokenInvalidException : Exception
{
    public VerifyTokenInvalidException() : base("Verify token is invalid")
    {
    }
}