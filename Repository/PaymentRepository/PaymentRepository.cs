using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace privaxnet_api.Repository.PaymentRepository;

public class PaymentRepository : IPaymentRepository
{
	private readonly DataContext _context;

    public PaymentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Payment> CreatePayment(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> GetPayment(Guid Id)
    {
        return await _context.Payments.FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<List<Payment>> GetPayments()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<List<Payment>> GetPaymentsByWallets(List<Wallet> wallets)
    {
        var allPayments = new List<Payment>();
        foreach (var wallet in wallets)
        {
            var payments = await _context.Payments.Where(u => u.WalletId == wallet.Id).ToListAsync();
            allPayments.AddRange(payments);
        }

        return allPayments;
    }


    public async Task<bool> PaymentExists(Guid Id)
    {
        return await _context.Payments.AnyAsync(x => x.Id == Id);
    }

    public async Task<bool> IsAproved(Payment pay)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(x => x.Id == pay.Id);
        return payment.IsAproved;
    }

    public async Task Aprove(Payment pay)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(x => x.Id == pay.Id);
        payment.IsAproved = true;
        await _context.SaveChangesAsync();
    }

    public async Task<decimal> CountEarns()
    {
        decimal earns = 0;
        var now = DateTime.Now.AddDays(-30);
        var payments = await _context.Payments.Where(x => x.DateUpdated >= now && x.IsAproved == true).ToListAsync();
        foreach (var payment in payments)
        {
            earns += payment.Amount;
        }

        return earns;
    }


    public async Task<decimal> CountEarnsToday()
    {
        decimal earns = 0;
        var now = DateTime.Now.AddDays(-1);
        var payments = await _context.Payments.Where(x => x.DateUpdated >= now && x.IsAproved == true).ToListAsync();
        foreach (var payment in payments)
        {
            earns += payment.Amount;
        }
        
        return earns;
    }

    public async Task<int> CountInvoices()
    {
        var now = DateTime.Now.AddDays(-30);
        var num = await _context.Payments.CountAsync(x => x.DateUpdated >= now);
        return num;
    }

    public async Task<int> CountPaid()
    {
        var now = DateTime.Now.AddDays(-30);
        var num = await _context.Payments.CountAsync(x => x.DateUpdated >= now && x.IsAproved == true);
        return num;
    }

}

