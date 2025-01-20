using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.PaymentService;

public interface IPaymentService
{
    Task<PaymentViewModel> CreatePayment(PaymentDto paymentDto);
    Task<List<PaymentViewModel>> GetPayments();
    Task<List<PaymentViewModel>> GetByUser(Guid userId);
    Task<List<PaymentViewModel>> GetMyPayments();
    Task<PaymentStatusViewModel> GetStatus();
    Task<User> AprovePayment(Guid Id);
}