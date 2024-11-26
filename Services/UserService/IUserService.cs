using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.UserService;

public interface IUserService
{
    Guid? GetId();
    Task<User> CreateUser(UserDto userDto);
    Task<List<User>> GetUsers();
    Task<User> GetUserById(Guid Id);
}