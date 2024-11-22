using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Services.AuthService;



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
            Name = userDto.Name,
            Email = userDto.Email
        };

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPost("all")]
    public async Task<ActionResult<List<UserViewModel>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);

    }
}
