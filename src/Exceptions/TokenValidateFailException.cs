namespace api.Exceptions;

public class TokenValidateFailException : Exception
{
    public TokenValidateFailException(string message = "Token validation failed") : base(message)
    {
    }
}