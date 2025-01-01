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

namespace privaxnet_api.Repository.WalletRepository;

public class WalletRepository : IWalletRepository
{
	private readonly DataContext _context;

    public WalletRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Wallet> CreateWallet(Wallet wallet)
    {
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();

        return wallet;
    }

    public async Task<List<Wallet>> GetWallets()
    {
        return await _context.Wallets.ToListAsync();
    }

    public async Task<List<Wallet>> GetMyWallets(Guid userId)
    {
        return await _context.Wallets.Where(w => w.UserId == userId).ToListAsync();
    }

    public async Task<List<Wallet>> GetWalletsByUser(User user)
    {
        return await _context.Wallets.Where(w => w.UserId == user.Id).ToListAsync();
    }

    public async Task<Wallet> GetWalletAsync(Guid Id)
    {
        return await _context.Wallets.FirstOrDefaultAsync(x => x.Id == Id);
    }

    public bool WalletExists(Guid Id)
    {
        return _context.Wallets.Any(x => x.Id == Id);
    }



}

