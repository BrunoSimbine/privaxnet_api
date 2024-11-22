using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Models;


public class UserDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; } = "Active";
    public string Role { get; set; } = "user";
    public string ClientId { get; set; }
    public string Phone { get; set; } = string.Empty;

    public Subscription Subscription { get; set; } 
}

public class Subscription
{
    public long DataAvaliable { get; set; }
    public long DataUsed { get; set; } 
    public DateTime Expires { get; set; } 
}