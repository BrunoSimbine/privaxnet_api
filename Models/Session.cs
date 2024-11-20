namespace privaxnet_api.Models;

public class Session
{
    public Guid Id { get; set; }

    public User User { get; set; }
    public Guid UserId { get; set; }

    public string Token { get; set; }

    public DateTime DataGen { get; set; } = DateTime.Now;
    public DateTime DataEng { get; set; }

    public string DeviceId { get; set; }
}
