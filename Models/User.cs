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

    [JsonIgnore]
    public byte[] PasswordHash { get; set; }

    [JsonIgnore]
    public byte[] PasswordSalt { get; set; }
}
