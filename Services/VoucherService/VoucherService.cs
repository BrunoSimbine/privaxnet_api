using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
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


    public VoucherService(DataContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<Voucher> CreateVoucher(VoucherDto voucherDto)
    {
        var agentId = _userService.GetId();

        return new Voucher();
    }

    public async Task<VoucherViewModel> UseVoucher(Guid Id)
    {
        return new VoucherViewModel();
    }

    public async Task<List<VoucherViewModel>> GetVouchers()
    {
        return new List<VoucherViewModel>();
    }

    public async Task<VoucherViewModel> GetVoucher(Guid Id)
    {
        return new VoucherViewModel();
    }


}

