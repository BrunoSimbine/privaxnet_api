using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Dtos;


public class ProductPriceDto
{
    public decimal Price { get; set; }
    public Guid ProductId { get; set; }
}
