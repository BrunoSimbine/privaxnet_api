namespace privaxnet_api.Exceptions;

public class VoucherNotFoundException : Exception
{
    public VoucherNotFoundException(string message) : base(message) { }
}
