namespace privaxnet_api.Exceptions;

public class InsuficientBalanceException : Exception
{
    public InsuficientBalanceException(string message) : base(message) { }
}
