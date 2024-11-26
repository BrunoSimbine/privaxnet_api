using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Models;


public class Product
{
    public Guid Id { get; set; }
    public bool IsAvaliable { get; set; } = true;
    public string Name { get; set; }
    public string ImageUrl { get; set; } = "https://statics.privaxnet.com/img/8273498325795.png";
    public decimal Price { get; set; }
    public long DataAmount { get; set; }
}
