namespace api.Exceptions;

public class AlreadyExistException : Exception
{
    public AlreadyExistException(string message = "Record already exist") : base(message) { }
}