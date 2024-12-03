namespace privaxnet_api.Exceptions;

public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException(string message) : base(message) { }
}
