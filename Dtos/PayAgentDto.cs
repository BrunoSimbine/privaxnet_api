using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Dtos;


public class PayAgentDto
{
    public string Name { get; set; }
    public string Account { get; set; }
    public Guid CurrencyId { get; set; }
}
