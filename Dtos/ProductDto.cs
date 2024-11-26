using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Dtos;


public class ProductDto
{
    public string Name { get; set; }
    public IFormFile Image { get; set; }
    public decimal Price { get; set; }
    public long DataAmount { get; set; }
}
