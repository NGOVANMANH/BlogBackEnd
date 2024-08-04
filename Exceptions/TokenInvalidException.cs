namespace api.Exceptions;

public class TokenInvalidException : Exception
{
    public TokenInvalidException() : base("Token is invalid")
    {
    }
}