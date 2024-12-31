using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Models;


public class User : BaseEntity
{
    public string Name { get; set; }
    public string Role { get; set; } = "user";
    public string ClientId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public decimal Balance { get; set; }
    public long DataAvailable { get; set; } = 262144;
    public long DataUsed { get; set; } 
    public DateTime ExpirationDate { get; set; } = DateTime.Now.AddHours(12);
    public bool IsBlocked
    {
        get
        {
            return DateDeleted.HasValue && DateDeleted.Value > DateTime.Now;
        }
    }

    public bool IsExpired
    {
        get
        {
            return ExpirationDate <= DateTime.Now;
        }
    }


    [JsonIgnore]
    public byte[] PasswordHash { get; set; }

    [JsonIgnore]
    public byte[] PasswordSalt { get; set; }

    [JsonIgnore]
    public string Token { get; set; } = string.Empty;
}
