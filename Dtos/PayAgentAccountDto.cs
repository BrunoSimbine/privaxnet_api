using System.Text.Json;
using System.Text.Json.Serialization;

namespace privaxnet_api.Dtos;


public class PayAgentAccountDto
{
    public string Account { get; set; }
    public Guid AgentId { get; set; }
}
