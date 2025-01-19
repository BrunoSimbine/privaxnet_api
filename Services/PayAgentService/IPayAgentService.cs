using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.PayAgentService;

public interface IPayAgentService
{
    Task<PayAgent> CreatePayAgent(PayAgentDto payAgentDto);
    Task<List<PayAgentViewModel>> GetPayAgentsAsync();
    Task<PayAgent> GetPayAgentAsync(Guid Id);
    Task<PayAgent> UpdateName(PayAgentNameDto payAgentName);
    Task<PayAgent> UpdateAccount(PayAgentAccountDto payAgentAccount);
    Task<List<PayAgentViewModel>> GetDeleted();
    Task<PayAgent> Recover(Guid Id);
    Task<PayAgent> Delete(Guid Id);
}