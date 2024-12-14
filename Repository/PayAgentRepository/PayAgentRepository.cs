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
        return await _context.PayAgents.ToListAsync();
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

}

