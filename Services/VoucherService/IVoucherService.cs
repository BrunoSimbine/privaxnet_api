using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
namespace privaxnet_api.Services.VoucherService;

public interface IVoucherService
{
    Task<Voucher> CreateVoucherAsync(VoucherDto voucherDto);
    Task<User> UseVoucherAsync(string Code);
    Task<List<VoucherViewModel>> GetVouchersAsync();
    Voucher GetVoucherByCode(string Code);
    Task<Voucher> GetVoucherByCodeAsync(string Code);
    Task<VoucherViewModel> GetVoucherAsync(Guid Id);
    Task<Voucher> CreateVoucherAsync(Guid ProductId);
    Task<List<VoucherViewModel>> GetUsed();
    Task<List<VoucherViewModel>> GetCreated();
}