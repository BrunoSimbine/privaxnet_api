namespace privaxnet_api.ViewModels;


public class VoucherViewModel
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Status { get; set; }

    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public Guid AgentId { get; set; }
    public string RequestPhone { get; set; }
}
