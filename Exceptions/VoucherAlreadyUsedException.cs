namespace privaxnet_api.Exceptions;

public class VoucherAlreadyUsedException : Exception
{
    public VoucherAlreadyUsedException(string message) : base(message) { }
}
