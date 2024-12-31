using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Dtos;


public class CurrencyDto
{
    public string Label { get; set; }
    public string LabelId { get; set; }
    
    public decimal Rate { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
}
