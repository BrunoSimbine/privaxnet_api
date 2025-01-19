using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.UserService;

public interface IUserService
{
    Task<User> CreateUserAsync(UserDto userDto);
    Task<List<User>> GetUsersAsync();
    Task<List<User>> GetActives();
    User GetUser();
    Task<User> SetRolesAsync(Guid Id, string role);
    Task<User> UpdateUserAsync(UserUpdateDto userUpdateDto);
    Task<User> GetUserAsync();
    Task<User> VerifyAsync();
    Task<bool> RechargeAsync(Voucher voucher);
    Task<User> AddConsuption(long data);
    User GetUserById(Guid Id);
    Task<User> GetUserByIdAsync(Guid Id);
    Task<User> AddDays(long days);
}