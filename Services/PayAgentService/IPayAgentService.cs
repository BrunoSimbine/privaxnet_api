using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.PayAgentService;

public interface IPayAgentService
{
    Task<PayAgent> CreatePayAgent(PayAgentDto payAgentDto);
    Task<List<PayAgent>> GetPayAgentsAsync();
    Task<PayAgent> GetPayAgentAsync(Guid Id);
}