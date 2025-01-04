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
using privaxnet_api.Repository.PaymentRepository;
using privaxnet_api.Repository.PayAgentRepository;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Services.PaymentService;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IPayAgentRepository _payAgentRepository;

    public PaymentService(IPaymentRepository paymentRepository, IUserRepository userRepository, ICurrencyRepository currencyRepository, IPayAgentRepository payAgentRepository, IWalletRepository walletRepository)
    {
        _paymentRepository = paymentRepository;
        _currencyRepository = currencyRepository;
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _payAgentRepository = payAgentRepository;
    }

    public async Task<PaymentViewModel> CreatePayment(PaymentDto paymentDto)
    {
        var wallet = await _walletRepository.GetWalletAsync(paymentDto.WalletId);
        var currency = await _currencyRepository.GetCurrencyAsync(wallet.CurrencyId);
        var payAgent = await _payAgentRepository.GetRandomPayAgentAsync(wallet.CurrencyId);

        var payment = await _paymentRepository.CreatePayment(new Payment {
            Amount = paymentDto.Amount,
            ExchangeAmount = paymentDto.Amount * currency.Rate,
            Wallet = wallet,
            PayAgent = payAgent
        });

        var paymentViewModel = new PaymentViewModel
        {
            Id = payment.Id,
            PaymentMethod = currency.Label,        
            Amount = payment.ExchangeAmount,

            CurrencySymbol = currency.Symbol,
            CurrencyName = currency.Name,

            AgentName = payAgent.Fullname,
            AgentAccount = payAgent.Account,

            UserName = wallet.Fullname,
            UserAccount = wallet.Account,

            IsAproved = payment.IsAproved
        };
        
        return paymentViewModel;
    }

    public async Task<List<PaymentViewModel>> GetPayments()
    {
        var payments = await _paymentRepository.GetPayments();
        var paymentsViewModel = new List<PaymentViewModel>();

        foreach (var payment in payments)
        {
            var wallet = await _walletRepository.GetWalletAsync(payment.WalletId);
            var currency = await _currencyRepository.GetCurrencyAsync(wallet.CurrencyId);
            var payAgent = await _payAgentRepository.GetPayAgentAsync(payment.PayAgentId);
            var user = await _userRepository.GetUserByIdAsync(wallet.UserId);

            paymentsViewModel.Add(new PaymentViewModel {
                Id = payment.Id,
                PaymentMethod = currency.Label,        
                Amount = payment.ExchangeAmount,

                CurrencySymbol = currency.Symbol,
                CurrencyName = currency.Name,

                AgentName = payAgent.Fullname,
                AgentAccount = payAgent.Account,

                UserName = wallet.Fullname,
                UserAccount = wallet.Account,

                IsAproved = payment.IsAproved
            });
        }

        return paymentsViewModel;
    }

    public async Task<List<PaymentViewModel>> GetByUser(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        var wallets = await _walletRepository.GetWalletsByUser(user);
        var payments = await _paymentRepository.GetPaymentsByWallets(wallets);
        var paymentsViewModel = new List<PaymentViewModel>();

        foreach (var payment in payments)
        {
            var wallet = await _walletRepository.GetWalletAsync(payment.WalletId);
            var currency = await _currencyRepository.GetCurrencyAsync(wallet.CurrencyId);
            var payAgent = await _payAgentRepository.GetPayAgentAsync(payment.PayAgentId);

            paymentsViewModel.Add(new PaymentViewModel {
                Id = payment.Id,
                PaymentMethod = currency.Label,        
                Amount = payment.ExchangeAmount,

                CurrencySymbol = currency.Symbol,
                CurrencyName = currency.Name,

                AgentName = payAgent.Fullname,
                AgentAccount = payAgent.Account,

                UserName = wallet.Fullname,
                UserAccount = wallet.Account,

                IsAproved = payment.IsAproved
            });
        }

        return paymentsViewModel;
    }

    public async Task<List<PaymentViewModel>> GetMyPayments()
    {
        var myUser = await _userRepository.GetUserAsync();
        var myWallets = await _walletRepository.GetWalletsByUser(myUser);
        var payments = await _paymentRepository.GetPaymentsByWallets(myWallets);
        var paymentsViewModel = new List<PaymentViewModel>();

        foreach (var payment in payments)
        {
            var wallet = await _walletRepository.GetWalletAsync(payment.WalletId);
            var currency = await _currencyRepository.GetCurrencyAsync(wallet.CurrencyId);
            var payAgent = await _payAgentRepository.GetPayAgentAsync(payment.PayAgentId);
            var user = await _userRepository.GetUserByIdAsync(wallet.UserId);

            paymentsViewModel.Add(new PaymentViewModel {
                Id = payment.Id,
                PaymentMethod = currency.Label,        
                Amount = payment.ExchangeAmount,

                CurrencySymbol = currency.Symbol,
                CurrencyName = currency.Name,

                AgentName = payAgent.Fullname,
                AgentAccount = payAgent.Account,

                UserName = wallet.Fullname,
                UserAccount = wallet.Account,

                IsAproved = payment.IsAproved
            });
        }

        return paymentsViewModel;
    }

    public async Task<User> AprovePayment(Guid Id)
    {
        var payment = await _paymentRepository.GetPayment(Id);
        var wallet = await _walletRepository.GetWalletAsync(payment.WalletId);
        var user = await _userRepository.GetUserByIdAsync(wallet.UserId);

        var isPaymentAvailable = await _paymentRepository.IsAproved(payment);
        if(!isPaymentAvailable)
        {
            var newUser = await _userRepository.AddBalanceAsync(user.Id, payment.Amount);
            await _paymentRepository.Aprove(payment);
            return newUser;
        }
        else{
            return new User();
        }

    }

}

