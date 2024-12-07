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

    public async Task<Voucher> CreateVoucherAsync(VoucherDto voucherDto)
    {
        var agentId = _userService.GetId();
        var agent = _userService.GetUserById(agentId);
        bool productExists = _context.Products.Any(x => x.Id == voucherDto.ProductId);
        if (productExists) {
            var product = _productService.GetProduct(voucherDto.ProductId);
            var voucher = new Voucher { Product = product, Agent = agent, RequestPhone = voucherDto.RequestPhone };
            voucher.Code = GenerateCode();
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        } else {
            throw new ProductNotFoundException("disu");
            return new Voucher();
        }

        

    }

    public async Task<bool> UseVoucherAsync(string Code)
    {
        var voucher = GetVoucherByCode(Code);
        if(voucher.Status == "Active"){
            var IsUsed = await _userService.RechargeAsync(voucher);
            if(IsUsed) {
                voucher.Status = "Inactive";
                voucher.UserId = _userService.GetId();
                await _context.SaveChangesAsync();
                return true;
            }
            throw new VoucherAlreadyUsedException("");
            return false;
        } else {
            throw new VoucherAlreadyUsedException("");
            return false;
        }
    }

    public async Task<List<VoucherViewModel>> GetVouchersAsync()
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

    public async Task<VoucherViewModel> GetVoucherAsync(Guid Id)
    {
        bool voucherExists = _context.Vouchers.Any(x => x.Id == Id);
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

    public async Task<Voucher> GetVoucherByCodeAsync(string Code)
    {
        bool voucherExists = _context.Vouchers.Any(x => x.Code == Code);
        if (voucherExists) {
            var voucher = await _context.Vouchers.Where(x => x.Code == Code)
            .FirstOrDefaultAsync();
            return voucher;
        }else{
            throw new VoucherNotFoundException("Voucher nao existe");
            return new Voucher();
        }
    }

    public Voucher GetVoucherByCode(string Code)
    {
        bool voucherExists = _context.Vouchers.Any(x => x.Code == Code);
        if (voucherExists) {
            var voucher = _context.Vouchers.Where(x => x.Code == Code)
            .FirstOrDefault();
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