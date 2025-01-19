using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Dtos;


public class RateDto
{
    public Guid CurrencyId { get; set; }
    public decimal Rate { get; set; }
}
