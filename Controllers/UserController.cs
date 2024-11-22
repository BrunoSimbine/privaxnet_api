using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text;



namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{    
    private readonly DataContext _context;
    private readonly IAuthService _authService;

    public UserController(DataContext context, IAuthService authService)
    {
        _authService = authService;
        _context = context;
    }
    [HttpPost("create")]
    public async Task<ActionResult<UserViewModel>> GetToken(UserDto userDto)
    {
        _authService.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var user = new User()
        {
            Name = userDto.Name
        };

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.ClientId = GuidToBase62(Guid.NewGuid());
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Created("https://privaxnet.com/users/get/1", user);
    }

    [HttpPost("all"), Authorize]
    public async Task<ActionResult<List<UserViewModel>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);

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

        return result.ToString().Substring(0, 6);
    }
}
