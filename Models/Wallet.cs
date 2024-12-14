using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Models;


public class Wallet : BaseEntity
{
    [JsonIgnore]
    public Currency Currency { get; set; }
    public Guid CurrencyId { get; set; }

    [JsonIgnore]
    public User User { get; set; }
    public Guid UserId { get; set; }

    public string Account { get; set; }
    public string FullName { get; set; }
}
