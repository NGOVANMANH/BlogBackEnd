namespace api.Exceptions
{
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException()
            : base("A user with this email or username already exists.") { }
    }
}
