using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.ViewModels;


public class PaymentViewModel
{
    public Guid Id { get; set; }

    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }

    public string CurrencySymbol { get; set; }
    public string CurrencyName { get; set; }

    public string AgentName { get; set; }
    public string AgentAccount { get; set; }

    public string UserName { get; set; }
    public string UserAccount { get; set; }

    public bool IsAproved { get; set; }
    public DateTime DateUpdated { get; set; }
}
