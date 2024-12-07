using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Models;

public class MessageVoucher
{
    public Guid Id { get; set; }
    public string Code { get; set; }

    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int DurationDays { get; set; }
    public long DataAmount { get; set; }

    public string UserName { get; set; }
    public string RequestPhone { get; set; }
}