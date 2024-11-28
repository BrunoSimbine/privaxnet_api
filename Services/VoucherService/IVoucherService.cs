using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
namespace privaxnet_api.Services.VoucherService;

public interface IVoucherService
{
    Task<Voucher> CreateVoucher(VoucherDto voucherDto);
    Task<bool> UseVoucher(string Code);
    Task<List<VoucherViewModel>> GetVouchers();
    Task<Voucher> GetVoucherByCode(string Code);
    Task<VoucherViewModel> GetVoucher(Guid Id);
}