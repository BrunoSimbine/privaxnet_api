
namespace privaxnet_api.Exceptions;

public class CurrencyLabelOrSKUAlreadyExistsException : Exception
{
    public CurrencyLabelOrSKUAlreadyExistsException(string message) : base(message) { }
}
