using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Models;


public class MessageUser
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
