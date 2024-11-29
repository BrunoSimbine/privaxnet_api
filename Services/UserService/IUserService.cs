using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.UserService;

public interface IUserService
{
    Guid GetId();
    Task<User> CreateUserAsync(UserDto userDto);
    Task<List<User>> GetUsersAsync();
    User GetUser();
    Task<User> SetRolesAsync(Guid Id, string role);
    Task<User> GetUserAsync();
    Task<bool> RechargeAsync(Voucher voucher);
    Task<bool> AddConsuption(long data);
    User GetUserById(Guid Id);
    Task<User> GetUserByIdAsync(Guid Id);
}