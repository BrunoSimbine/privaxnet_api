using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Repository.PaymentRepository;

public interface IPaymentRepository
{
	Task<Payment> CreatePayment(Payment payment);
	Task<Payment> GetPayment(Guid Id);
	Task<List<Payment>> GetPayments();
	Task<List<Payment>> GetPaymentsByWallets(List<Wallet> wallets);
	Task<bool> PaymentExists(Guid Id);
	Task<bool> IsAproved(Payment pay);
	Task Aprove(Payment pay);
	Task<int> CountPaid();
	Task<int> CountInvoices();
	Task<decimal> CountEarnsToday();
	Task<decimal> CountEarns();
}