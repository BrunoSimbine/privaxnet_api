using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Models;


public class PayAgent : BaseEntity
{
    public string Fullname { get; set; }
    public string Account { get; set; }

    [JsonIgnore]
    public Currency Currency { get; set; }
    public Guid CurrencyId { get; set; }
}
