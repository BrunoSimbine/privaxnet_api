using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Dtos;


public class PayAgentNameDto
{
    public string Name { get; set; }
    public Guid AgentId { get; set; }
}
