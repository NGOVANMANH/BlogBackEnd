public class UserNotExistException : Exception
{
    public UserNotExistException()
        : base("User does not exist.") { }
}