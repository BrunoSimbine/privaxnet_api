using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.PaymentService;

public interface IPaymentService
{
    Task<PaymentViewModel> CreatePayment(PaymentDto paymentDto);
    Task<List<PaymentViewModel>> GetPayments();
    Task<List<PaymentViewModel>> GetMyPayments();
    Task<User> AprovePayment(Guid Id);
}