using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
using privaxnet_api.Services.UserService;
using privaxnet_api.Repository.UserRepository;
using privaxnet_api.Services.ProductService;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Repository.VoucherRepository;

public class VoucherRepository : IVoucherRepository
{
    private readonly DataContext _context;


    public VoucherRepository(DataContext context)
    {
        _context = context;

    }

    public async Task<Voucher> CreateVoucherAsync(Voucher voucher)
    {
        _context.Vouchers.Add(voucher);
        await _context.SaveChangesAsync();
        return voucher;
    }

    public async Task<bool> UseVoucherAsync(string Code)
    {
        var voucher = GetVoucherByCode(Code);
        voucher.IsUsed = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<VoucherViewModel>> GetVouchersAsync()
    {
        var vouchers = await _context.Vouchers.Select(x => new VoucherViewModel {
            Id = x.Id,
            Code = x.Code,
            IsUsed = x.IsUsed,
            ProductId = x.ProductId,
            UserId = x.UserId,
            AgentId = x.AgentId,
            RequestPhone = x.RequestPhone
        }).ToListAsync();

        return vouchers;
    }

    public async Task<VoucherViewModel> GetVoucherAsync(Guid Id)
    {
        var voucher = await _context.Vouchers.Where(x => x.Id == Id)
        .Select(x => new VoucherViewModel 
        {
            Id = x.Id,
            Code = x.Code,
            IsUsed = x.IsUsed,
            ProductId = x.ProductId,
            UserId = x.UserId,
            AgentId = x.AgentId,
            RequestPhone = x.RequestPhone
        }).FirstOrDefaultAsync();

        return voucher;
    }

    public async Task<List<VoucherViewModel>> GetUsed(Guid userId)
    {
        var voucher = await _context.Vouchers.Where(x => x.UserId == userId)
        .Select(x => new VoucherViewModel 
        {
            Id = x.Id,
            Code = x.Code,
            IsUsed = x.IsUsed,
            ProductId = x.ProductId,
            UserId = x.UserId,
            AgentId = x.AgentId,
            RequestPhone = x.RequestPhone
        }).ToListAsync();

        return voucher;
    }

    public async Task<List<VoucherViewModel>> GetCreated(Guid agentId)
    {
        var voucher = await _context.Vouchers.Where(x => x.AgentId == agentId)
        .Select(x => new VoucherViewModel 
        {
            Id = x.Id,
            Code = x.Code,
            IsUsed = x.IsUsed,
            ProductId = x.ProductId,
            UserId = x.UserId,
            AgentId = x.AgentId,
            RequestPhone = x.RequestPhone
        }).ToListAsync();

        return voucher;
    }

    public async Task<Voucher> GetVoucherByCodeAsync(string Code)
    {
        var voucher = await _context.Vouchers.Where(x => x.Code == Code)
        .FirstOrDefaultAsync();
        return voucher;
    }

    public Voucher GetVoucherByCode(string Code)
    {
        var voucher = _context.Vouchers.Where(x => x.Code == Code)
        .FirstOrDefault();
        return voucher;
    }

    public bool VoucherExists(string Code)
    {
        var voucherExists = _context.Vouchers.Any(x => x.Code == Code);
        return voucherExists;
    }

    public async Task SaveChangesAsync() 
    {
        await _context.SaveChangesAsync();
    }

    public string GenerateCode()
    {
        Random random = new Random();

        StringBuilder result = new StringBuilder();

        for (int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                char letra = (char)random.Next('A', 'F' + 1);
                result.Append(letra);
            }

            for(int j = 0; j < 2; j++)
            {
                char numero = (char)random.Next('0', '9' + 1);
                result.Append(numero);
            }
        }

        return result.ToString();
    }
}