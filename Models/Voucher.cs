using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Models;


public class Voucher
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Status { get; set; } = "Active"; 

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UsedAt { get; set; }

    public Product Product { get; set; }
    public Guid ProductId { get; set; }

    public User Agent { get; set; }
    public Guid AgentId { get; set; }

    public User User { get; set; }
    public Guid UserId { get; set; }

    public string RequestPhone { get; set; }
}
