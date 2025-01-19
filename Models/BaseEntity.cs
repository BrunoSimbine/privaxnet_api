using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Models;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime DateUpdated { get; set; } = DateTime.Now;

    [JsonIgnore]
    public DateTime? DateDeleted { get; set; }
}
