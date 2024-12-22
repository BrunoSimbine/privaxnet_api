using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace privaxnet_api.Repository.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly IHttpContextAccessor _accessor;
	private readonly DataContext _context;

    public UserRepository(IHttpContextAccessor accessor, DataContext context)
    {
        _accessor = accessor;
        _context = context;
    }

    public Guid GetId()
    {
        var id = _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid);
        return Guid.Parse(id);
    }

    public string GetCurrentToken()
    {
        var headers = _accessor.HttpContext?.Request.Headers;
        return headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
    }


    public async Task<User> CreateUserAsync(UserDto userDto)
    {
        var user = new User();
        CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.Name = userDto.Name;
        user.Phone = userDto.Phone;
        user.Email = userDto.Email;

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.ClientId = GuidToBase62(Guid.NewGuid());

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;            
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users; // Amizade2020.z
    }

    public async Task<User> GetUserByIdAsync(Guid Id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<User> SetRolesAsync(Guid Id, string role)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == Id);
        user.Role = role;
        await _context.SaveChangesAsync();
        return user;
    }

    public User GetUserById(Guid Id)
    {

        var user = _context.Users.FirstOrDefault(x => x.Id == Id);
        return user;

    }

    public async Task<User> GetUserAsync()
    {
        var Id = GetId();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
        return user;
    }

    public async Task<User> GetUserByNameAsync(string Name)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == Name);
        return user;
    }

    public async Task<User> UpdateUserAsync(UserUpdateDto userUpdateDto)
    {
        var Id = GetId();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

        user.Email = userUpdateDto.Email;
        user.Phone = userUpdateDto.Phone;
        await _context.SaveChangesAsync();
        return user;

    }

    public User GetUser()
    {
        var Id = GetId();
        var user = _context.Users.FirstOrDefault(x => x.Id == Id);
        return user;
    }

    public async Task<User> RechargeAsync(long data, int duration)
    {
        var Id = GetId();
        var user = await GetUserByIdAsync(Id);
        user.ExpirationDate = DateTime.Now.AddDays(duration);
        user.DataAvailable += data;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> AddConsuption(long data) 
    {
        var id = GetId();
        var user = await GetUserByIdAsync(id);
        if (user.DataAvailable >= data) {
            user.DataAvailable -= data;
            user.DataUsed += data;
            await _context.SaveChangesAsync();
            return true;
        } else {
            user.DataUsed += user.DataAvailable;
            user.DataAvailable = 0;
            await _context.SaveChangesAsync();
            return false;
        }
    }

    public async Task<User> AddBalanceAsync(Guid userId, decimal balance)
    {
        var user = await GetUserByIdAsync(userId);
        user.Balance += balance;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateToken(Guid userId, string token)
    {
        var user = await GetUserByIdAsync(userId);
        user.Token = token;
        await _context.SaveChangesAsync();
        return user;
    }


    public bool NameExists(string Name) 
    {
        return _context.Users.Any(x => x.Name == Name);
    }

    public bool EmailExists(string Email) 
    {
        return _context.Users.Any(x => x.Email == Email);
    }

    public bool PhoneExists(string Phone) 
    {
        return _context.Users.Any(x => x.Phone == Phone);
    }

    public bool UserExists(Guid Id) 
    {
        return _context.Users.Any(x => x.Id == Id);
    }

    public async Task SaveChangesAsync() 
    {
        await _context.SaveChangesAsync();
    }



    private string GuidToBase62(Guid guid) {
        var base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";
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

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA256();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA256(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Sid, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("jwfhgfhjsgdvjhdsg837483hf8743tfg8734gfyegf7634gf38734"));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(claims: claims, expires:DateTime.Now.AddDays(2), signingCredentials:cred);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public bool IsTokenValid(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        if(jwtHandler.CanReadToken(token)){
            var jwtToken = jwtHandler.ReadToken(token);

            var expiration = jwtToken.ValidTo;
            return DateTime.UtcNow < expiration;
        }

        return false;
    }

    public DateTime TokenExpires(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        if(jwtHandler.CanReadToken(token)){
            var jwtToken = jwtHandler.ReadToken(token);

            var expiration = jwtToken.ValidTo;
            return expiration;
        }
        return DateTime.UtcNow;
    }


}

