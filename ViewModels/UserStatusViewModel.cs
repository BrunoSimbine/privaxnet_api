using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.ViewModels;

public class UserStatusViewModel
{
    public int OnlineNow { get; set; }
    public int OnlineToday { get; set; }
    public int Active { get; set; }
    public int All { get; set; }
}
