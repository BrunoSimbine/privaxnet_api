using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
namespace privaxnet_api.Repository.VoucherRepository;

public interface IVoucherRepository
{
    Task<bool> UseVoucherAsync(string Code);
    Task<List<VoucherViewModel>> GetVouchersAsync();
    Voucher GetVoucherByCode(string Code);
    Task<Voucher> GetVoucherByCodeAsync(string Code);
    Task<VoucherViewModel> GetVoucherAsync(Guid Id);
    Task<Voucher> CreateVoucherAsync(Voucher voucher);
    string GenerateCode();
    bool VoucherExists(string Code);
    Task SaveChangesAsync();
}