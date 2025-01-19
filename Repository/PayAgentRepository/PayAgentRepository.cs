using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace privaxnet_api.Repository.PayAgentRepository;

public class PayAgentRepository : IPayAgentRepository
{
	private readonly DataContext _context;

    public PayAgentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<PayAgent> CreatePayAgent(PayAgent payAgent)
    {
        _context.PayAgents.Add(payAgent);
        await _context.SaveChangesAsync();

        return payAgent;
    }

    public async Task<List<PayAgent>> GetPayAgentsAsync()
    {
        return await _context.PayAgents.Where(x => x.DateDeleted == null).ToListAsync();
    } 

    public async Task<List<PayAgent>> GetDeleted()
    {
        return await _context.PayAgents.Where(x => x.DateDeleted != null).ToListAsync();
    } 

    public async Task<PayAgent> GetPayAgentAsync(Guid Id)
    {
        return await _context.PayAgents.FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<bool> PayAgentExists(Guid Id)
    {
        return await _context.PayAgents.AnyAsync(x => x.Id == Id);
    }

    public async Task<PayAgent> GetRandomPayAgentAsync(Guid CurrencyId)
    {
        return await _context.PayAgents
        .Where(x => x.CurrencyId == CurrencyId)
        .OrderBy(x => Guid.NewGuid())
        .FirstOrDefaultAsync();
    }

    public async Task<PayAgent> UpdateName(PayAgentNameDto payAgentName)
    {
        var payAgent = await _context.PayAgents.FirstOrDefaultAsync(x => x.Id == payAgentName.AgentId);
        payAgent.DateUpdated = DateTime.Now;
        payAgent.Fullname = payAgentName.Name;
        await _context.SaveChangesAsync();
        return payAgent;
    }

    public async Task<PayAgent> UpdateAccount(PayAgentAccountDto payAgentAccount)
    {
        var payAgent = await _context.PayAgents.FirstOrDefaultAsync(x => x.Id == payAgentAccount.AgentId);
        payAgent.DateUpdated = DateTime.Now;
        payAgent.Account = payAgentAccount.Account;
        await _context.SaveChangesAsync();
        return payAgent;
    }

    public async Task<PayAgent> Recover(Guid Id)
    {
        var payAgent = await _context.PayAgents.FirstOrDefaultAsync(x => x.Id == Id);
        payAgent.DateUpdated = DateTime.Now;
        payAgent.DateDeleted = null;
        await _context.SaveChangesAsync();
        return payAgent;
    }

    public async Task<PayAgent> Delete(Guid Id)
    {
        var payAgent = await _context.PayAgents.FirstOrDefaultAsync(x => x.Id == Id);
        payAgent.DateUpdated = DateTime.Now;
        payAgent.DateDeleted = DateTime.Now;
        await _context.SaveChangesAsync();
        return payAgent;
    }


}

