using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Models;


public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; } = "Active";
    public string Role { get; set; } = "user";
    public string ClientId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public int Balance { get; set; }
    public long DataAvaliable { get; set; }
    public long DataUsed { get; set; } 
    public DateTime Expires { get; set; } = DateTime.Now.ToUniversalTime();


    [JsonIgnore]
    public byte[] PasswordHash { get; set; }

    [JsonIgnore]
    public byte[] PasswordSalt { get; set; }
}
