using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Models;


public class Currency : BaseEntity
{
    public string Label { get; set; }
    public string LabelId { get; set; }
    
    public decimal Rate { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
}

