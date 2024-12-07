namespace privaxnet_api.Exceptions;

public class InvalidWhatsAppPhoneException : Exception
{
    public InvalidWhatsAppPhoneException(string message) : base(message) { }
}
