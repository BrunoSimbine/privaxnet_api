using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
namespace privaxnet_api.Services.VoucherService;

public interface IVoucherService
{
    Task<Voucher> CreateVoucherAsync(VoucherDto voucherDto);
    Task<bool> UseVoucherAsync(string Code);
    Task<List<VoucherViewModel>> GetVouchersAsync();
    Voucher GetVoucherByCode(string Code);
    Task<Voucher> GetVoucherByCodeAsync(string Code);
    Task<VoucherViewModel> GetVoucherAsync(Guid Id);
}