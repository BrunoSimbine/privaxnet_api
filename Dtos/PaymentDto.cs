using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Dtos;


public class PaymentDto
{
    public decimal Amount { get; set; }
    public Guid WalletId { get; set; }
}
