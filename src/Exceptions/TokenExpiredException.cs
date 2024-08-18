namespace api.Exceptions;

public class TokenExpiredException : Exception
{
    public TokenExpiredException(string message = "Token is expired") : base(message)
    {
    }
}