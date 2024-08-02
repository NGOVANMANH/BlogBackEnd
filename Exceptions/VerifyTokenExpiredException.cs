namespace api.Exceptions;

public class VerifyTokenExpiredException : Exception
{
    public VerifyTokenExpiredException() : base("Verify token is expired")
    {
    }
}