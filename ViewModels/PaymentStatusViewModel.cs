using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.ViewModels;

public class PaymentStatusViewModel
{
    public decimal Earns { get; set; }
    public decimal EarnsToday { get; set; }
    public int Invoices { get; set; }
    public int Paid { get; set; }
}
