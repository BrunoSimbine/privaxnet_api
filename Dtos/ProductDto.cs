using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Dtos;


public class ProductDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
}
