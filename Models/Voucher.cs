using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Models;


public class Voucher: BaseEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public bool IsUsed { get; set; } = false; 

    public Product Product { get; set; }
    public Guid ProductId { get; set; }

    public User Agent { get; set; }
    public Guid AgentId { get; set; }

    public Guid UserId { get; set; }

    public string RequestPhone { get; set; }
}
