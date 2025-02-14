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
    [JsonIgnore]
    public long DataAvailable { get; set; } = 262144;
    [JsonIgnore]
    public long DataUsed { get; set; } 
    public DateTime ExpirationDate { get; set; } = DateTime.Now.AddHours(12);
    public DateTime LastActivity { get; set; } = DateTime.Now;

    public bool IsOnline
    {
        get
        {
            return (DateTime.Now - LastActivity).Duration().TotalMinutes < 5;
        }
    }
    public bool IsDeleted
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
