using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using System.Security.Claims;
using privaxnet_api.Models;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.AuthService;

using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace privaxnet_api.Services.UserService;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _accessor;
	private readonly DataContext _context;
    private readonly IAuthService _authService;

    public UserService(IHttpContextAccessor accessor, DataContext context, IAuthService authService)
    {
        _accessor = accessor;
        _authService = authService;
        _context = context;
    }

    public Guid? GetId()
    {
        var id = _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid);
        return Guid.Parse(id!);
    }

    public async Task<User> CreateUser(UserDto userDto)
    {
		var user = new User();
		bool userExists = await _context.Users.AnyAsync(x => x.Name == userDto.Name);
        if (!userExists) {
            _authService.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            
            user.Name = userDto.Name;
            user.Phone = userDto.Phone;
           

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ClientId = GuidToBase62(Guid.NewGuid());
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }else {
            throw new UserAlreadyExistsException("Usuario ja existe!");
        	return user;
        }
    }

    public async Task<List<User>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users;

    }

    public async Task<User> GetUserById(Guid Id)
    {
    	var user = new User();
        bool userExists = await _context.Users.AnyAsync(x => x.Id == Id);
        if (userExists) {
            user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            return user;
        }else{
        	throw new UserNotFoundException("Usuario nao encontrado!");
        	return user;
        }
    }


    private string GuidToBase62(Guid guid) {
        var base62Chars = "0123456789abcdefghijklmnopqestuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        byte[] bytes = guid.ToByteArray();
        long number = BitConverter.ToInt64(bytes, 0);

        if (number < 0 ) throw new ArgumentOutOfRangeException(nameof(number), "Numero deve ser positivo.");

        var result = new StringBuilder();

        do {
            int remainder = (int)(number % 62);
            result.Insert(0, base62Chars[remainder]);
            number /= 62;
        } while (number > 0);

        return result.ToString().Substring(1, 7);
    }
}

