namespace api.Exceptions;

public class CreatedFailException : Exception
{
    public CreatedFailException(string message = "Record create fail") : base(message) { }
}