using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Models;


public class User : BaseEntity
{
    public string Name { get; set; }
    public string Status { get; set; } = "Active";
    public string Role { get; set; } = "admin";
    public string ClientId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public decimal Balance { get; set; }
    public long DataAvailable { get; set; } = 262144;
    public long DataUsed { get; set; } 
    public DateTime ExpirationDate { get; set; }


    [JsonIgnore]
    public byte[] PasswordHash { get; set; }

    [JsonIgnore]
    public byte[] PasswordSalt { get; set; }
}
