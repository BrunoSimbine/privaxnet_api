using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Repository.PayAgentRepository;

public interface IPayAgentRepository
{
	Task<PayAgent> CreatePayAgent(PayAgent payAgent);
	Task<bool> PayAgentExists(Guid Id);
	Task<List<PayAgent>> GetPayAgentsAsync();
	Task<PayAgent> GetPayAgentAsync(Guid Id);
	Task<PayAgent> GetRandomPayAgentAsync(Guid CurrencyId);
}