using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.MessageService;

public interface IMessageService
{
    Task<string> SendWelcomeAsync(MessageUser user);
    Task<string> SendVoucherAsync(MessageVoucher voucher);
}