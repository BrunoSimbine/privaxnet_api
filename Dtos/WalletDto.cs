using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Dtos;


public class WalletDto
{
    public Guid CurrencyId { get; set; }
    public string Account { get; set; }
    public string Fullname { get; set; }
}
