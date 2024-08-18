namespace api.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message = "Record not found") : base(message) { }
}