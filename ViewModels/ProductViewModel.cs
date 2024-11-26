using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.ViewModels;


public class ProductViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; } = "https://statics.privaxnet.com/img/8273498325795.png";
    public decimal Price { get; set; }
    public long DataAmount { get; set; }
}
