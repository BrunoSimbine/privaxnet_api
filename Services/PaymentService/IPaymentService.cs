using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.PaymentService;

public interface IPaymentService
{
    Task<Payment> CreatePayment(PaymentDto paymentDto);
    Task<List<Payment>> GetPayments();
    Task<User> AprovePayment(Guid Id);
}