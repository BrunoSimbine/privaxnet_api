namespace privaxnet_api.ViewModels;

public class SessionViewModel
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public bool IsValid { get; set; }
    public DateTime Expires { get; set; }
}
