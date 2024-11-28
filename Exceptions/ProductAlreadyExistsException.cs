namespace privaxnet_api.Exceptions;

public class ProductAlreadyExistsException : Exception
{
    public ProductAlreadyExistsException(string message) : base(message) { }
}
