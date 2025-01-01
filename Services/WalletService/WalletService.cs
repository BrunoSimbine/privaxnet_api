using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.ViewModels;
using privaxnet_api.Repository.WalletRepository;
using privaxnet_api.Repository.UserRepository;
using privaxnet_api.Repository.CurrencyRepository;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Services.WalletService;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyRepository _currencyRepository;

    public WalletService(IWalletRepository walletRepository, IUserRepository userRepository, ICurrencyRepository currencyRepository)
    {
        _walletRepository = walletRepository;
        _currencyRepository = currencyRepository;
        _userRepository = userRepository;
    }

    public async Task<Wallet> CreateWallet(WalletDto walletDto)
    {
        var user = _userRepository.GetUser();
        var currency = await _currencyRepository.GetCurrencyAsync(walletDto.CurrencyId);


        var wallet = await _walletRepository.CreateWallet(new Wallet {
            User = user,
            Currency = currency,
            Account = walletDto.Account,
            Fullname = walletDto.Fullname
        });

        return wallet;
    }

    public async Task<List<Wallet>> GetWallets()
    {
        var wallets = await _walletRepository.GetWallets();
        return wallets;
    }

    public async Task<List<Wallet>> GetMyWallets()
    {
        var userId = _userRepository.GetId();
        var wallets = await _walletRepository.GetMyWallets(userId);
        return wallets;
    }

    public async Task<Wallet> GetWallet(Guid Id)
    {
        var walletExists = _walletRepository.WalletExists(Id);

        if (walletExists)
        {
            var wallet = await _walletRepository.GetWalletAsync(Id);
            return wallet;
        } else {
            throw new WalletNotFoundException("Wallet not found");
            return new Wallet();
        }

    }

}

