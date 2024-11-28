using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
using privaxnet_api.Services.UserService;
using privaxnet_api.Services.ProductService;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Services.VoucherService;

public class VoucherService : IVoucherService
{

    private readonly DataContext _context;
    private readonly IUserService _userService;
    private readonly IProductService _productService;


    public VoucherService(DataContext context, IUserService userService, IProductService productService)
    {
        _context = context;
        _userService = userService;
        _productService = productService;
    }

    public async Task<Voucher> CreateVoucher(VoucherDto voucherDto)
    {
        var agentId = _userService.GetId();
        var agent = await _userService.GetUserById(agentId);
        var product = await _productService.GetProduct(voucherDto.ProductId);
        var voucher = new Voucher { Product = product, Agent = agent, RequestPhone = voucherDto.RequestPhone };
        voucher.Code = GenerateCode();
        _context.Vouchers.Add(voucher);
        await _context.SaveChangesAsync();
        return voucher;
    }

    public async Task<bool> UseVoucher(string Code)
    {
        var voucher = await GetVoucherByCode(Code);
        var IsUsed = await _userService.Recharge(voucher);
        if(IsUsed) {
            voucher.Status = "Inactive";
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<List<VoucherViewModel>> GetVouchers()
    {
        var vouchers = await _context.Vouchers.Select(x => new VoucherViewModel {
            Id = x.Id,
            Code = x.Code,
            Status = x.Status,
            ProductId = x.ProductId,
            UserId = x.UserId,
            AgentId = x.AgentId,
            RequestPhone = x.RequestPhone
        }).ToListAsync();

        return vouchers;
    }

    public async Task<VoucherViewModel> GetVoucher(Guid Id)
    {
        bool voucherExists = await _context.Vouchers.AnyAsync(x => x.Id == Id);
        if (voucherExists) {
            var voucher = await _context.Vouchers.Where(x => x.Id == Id)
            .Select(x => new VoucherViewModel 
            {
                Id = x.Id,
                Code = x.Code,
                Status = x.Status,
                ProductId = x.ProductId,
                UserId = x.UserId,
                AgentId = x.AgentId,
                RequestPhone = x.RequestPhone
            }).FirstOrDefaultAsync();

            return voucher;
        }else{
            throw new VoucherNotFoundException("Voucher nao existe");
            return new VoucherViewModel();
        }
    }

    public async Task<Voucher> GetVoucherByCode(string Code)
    {
        bool voucherExists = await _context.Vouchers.AnyAsync(x => x.Code == Code);
        if (voucherExists) {
            var voucher = await _context.Vouchers.Where(x => x.Code == Code)
            .FirstOrDefaultAsync();

            return voucher;
        }else{
            throw new VoucherNotFoundException("Voucher nao existe");
            return new Voucher();
        }
    }

    private string GenerateCode(){
        string chars = "ABCDEFGHIJ1234567890";

        Random random = new Random();
        return new string(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray());
    }


}

