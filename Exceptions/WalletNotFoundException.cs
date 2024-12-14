namespace privaxnet_api.Exceptions;

public class WalletNotFoundException : Exception
{
    public WalletNotFoundException(string message) : base(message) { }
}
