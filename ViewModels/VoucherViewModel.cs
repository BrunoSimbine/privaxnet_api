namespace privaxnet_api.ViewModels;


public class VoucherViewModel
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Status { get; set; }


    public DateTime CreatedAt { get; set; }
    public DateTime UsedAt { get; set; }

}
