using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Repository.UserRepository;

public interface IUserRepository
{
    Task<User> CreateUserAsync(UserDto userDto);  
    Guid GetId();
    Task<List<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(Guid Id);
    Task<User> SetRolesAsync(Guid Id, string role);
    User GetUserById(Guid Id);
    Task<User> GetUserAsync();
    Task<User> UpdateUserAsync(UserUpdateDto userUpdateDto);
    User GetUser();
    Task<User> RechargeAsync(long data, int duration);
    Task<bool> AddConsuption(long data);
    bool NameExists(string Name);
    bool EmailExists(string Email);
    bool PhoneExists(string Phone);
    bool UserExists(Guid Id);
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    bool IsTokenValid(string token);
    string CreateToken(User user);
    Task<User> GetUserByNameAsync(string Name);
    DateTime TokenExpires(string token);
    Task SaveChangesAsync();
    Task<User> AddBalanceAsync(Guid userId, decimal balance);
}