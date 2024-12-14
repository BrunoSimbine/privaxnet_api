using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
namespace privaxnet_api.Repository.WalletRepository;

public interface IWalletRepository
{
    Task<Wallet> CreateWallet(Wallet wallet);
    Task<List<Wallet>> GetWallets();
    Task<Wallet> GetWalletAsync(Guid Id);
    bool WalletExists(Guid Id);
}