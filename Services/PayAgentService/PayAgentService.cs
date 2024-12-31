using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.ViewModels;
using privaxnet_api.Repository.WalletRepository;
using privaxnet_api.Repository.UserRepository;
using privaxnet_api.Repository.CurrencyRepository;
using privaxnet_api.Repository.PayAgentRepository;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Services.PayAgentService;

public class PayAgentService : IPayAgentService
{
    private readonly IPayAgentRepository _payAgentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyRepository _currencyRepository;

    public PayAgentService(IPayAgentRepository payAgentRepository, IUserRepository userRepository, ICurrencyRepository currencyRepository)
    {
        _payAgentRepository = payAgentRepository;
        _currencyRepository = currencyRepository;
        _userRepository = userRepository;
    }

    public async Task<PayAgent> CreatePayAgent(PayAgentDto payAgentDto)
    {
        var currency = await _currencyRepository.GetCurrencyAsync(payAgentDto.CurrencyId);
        var payAgent = await _payAgentRepository.CreatePayAgent(new PayAgent {
            Currency = currency,
            Fullname = payAgentDto.Fullname,
            Account = payAgentDto.Account
        });

        return payAgent;
    }

    public async Task<List<PayAgent>> GetPayAgentsAsync()
    {
        return await _payAgentRepository.GetPayAgentsAsync();
    }

    public async Task<PayAgent> GetPayAgentAsync(Guid Id)
    {
        var payAgentExists = await _payAgentRepository.PayAgentExists(Id);
        if (payAgentExists)
        {
            return await _payAgentRepository.GetPayAgentAsync(Id); 
        } else {
            throw new PayAgentNotFoundException("Psl");
            return new PayAgent();
        }
        
    }


}

