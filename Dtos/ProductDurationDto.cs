using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Dtos;


public class ProductDurationDto
{
    public int Duration { get; set; }
    public Guid ProductId { get; set; }
}
