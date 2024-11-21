
namespace privaxnet_api.Models;


public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Status { get; set; } = "Active";
}
