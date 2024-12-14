using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Models;


public class Currency : BaseEntity
{
    public string Label { get; set; }
    public string LabelId { get; set; }
    
    public decimal ExchangeRate { get; set; }
    public string CurrencySymbol { get; set; }
    public string CurrencyName { get; set; }
}
