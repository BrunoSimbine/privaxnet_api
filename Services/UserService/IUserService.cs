using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.UserService;

public interface IUserService
{
    Guid GetId();
    Task<User> CreateUser(UserDto userDto);
    Task<List<User>> GetUsers();
    Task<bool> Recharge(Voucher voucher);
    Task<bool> AddConsuption(long data);
    Task<User> GetUserById(Guid Id);
}