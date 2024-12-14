using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.WalletService;

public interface IWalletService
{
	Task<Wallet> CreateWallet(WalletDto walletDto);
	Task<List<Wallet>> GetWallets();
	Task<Wallet> GetWallet(Guid Id);
}