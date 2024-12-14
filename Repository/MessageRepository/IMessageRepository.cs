using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Repository.MessageRepository;

public interface IMessageRepository
{
    Task<string> SendWelcomeAsync(MessageUser user);
    Task<string> SendVoucherAsync(MessageVoucher voucher);
}