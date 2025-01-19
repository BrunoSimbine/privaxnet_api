using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.ViewModels;

public class PayAgentViewModel
{
    public Guid Id { get; set; }

    public string Fullname { get; set; }
    public string Account { get; set; }

    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }

    public string Method { get; set; }
}
