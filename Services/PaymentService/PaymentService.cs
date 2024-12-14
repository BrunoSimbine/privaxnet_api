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

    public async Task<Payment> CreatePayment(PaymentDto paymentDto)
    {
        var wallet = await _walletRepository.GetWalletAsync(paymentDto.WalletId);
        var currency = await _currencyRepository.GetCurrencyAsync(wallet.CurrencyId);
        var payAgent = await _payAgentRepository.GetRandomPayAgentAsync(wallet.CurrencyId);

        var payment = await _paymentRepository.CreatePayment(new Payment {
            Amount = paymentDto.Amount,
            ExchangeAmount = paymentDto.Amount * currency.ExchangeRate,
            Wallet = wallet,
            PayAgent = payAgent
        });

        return payment;
    }

    public async Task<List<Payment>> GetPayments()
    {
        return await _paymentRepository.GetPayments();
    }

    public async Task<User> AprovePayment(Guid Id)
    {
        var payment = await _paymentRepository.GetPayment(Id);
        var wallet = await _walletRepository.GetWalletAsync(payment.WalletId);
        var user = await _userRepository.GetUserByIdAsync(wallet.UserId);

        var newUser = await _userRepository.AddBalanceAsync(user.Id, payment.Amount);
        return newUser;
    }

}

