namespace api.Exceptions;

public class UserNotVerifiedException : Exception
{
    public UserNotVerifiedException() : base("User is not verified")
    {
    }
}