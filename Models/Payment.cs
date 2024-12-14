using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Models;


public class Payment : BaseEntity
{
    public decimal Amount { get; set; }
    public decimal ExchangeAmount { get; set; }

    public bool IsAproved { get; set; } = false;
    public DateTime ExpirationDate { get; set; } = DateTime.Now.AddMinutes(45);

    [JsonIgnore]
    public PayAgent PayAgent { get; set; }
    public Guid PayAgentId { get; set; }

    [JsonIgnore]
    public Wallet Wallet { get; set; }
    public Guid WalletId { get; set; }
}
 