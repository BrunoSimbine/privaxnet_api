
namespace privaxnet_api.Models;


public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; } = "Active";
    public string Role { get; set; } = "user";
    public string ClientId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}
