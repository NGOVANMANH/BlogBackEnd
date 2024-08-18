namespace api.Exceptions;

public class TokenInvalidException : Exception
{
    public TokenInvalidException(string message = "Token is invalid") : base(message)
    {
    }
}