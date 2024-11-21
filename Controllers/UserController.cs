using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;


namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{    
    private readonly DataContext _context;

    public UserController(DataContext context)
    {
        _context = context;
    }
    [HttpPost("create")]
    public async  Task<ActionResult<UserViewModel>> GetToken(UserDto userDto)
    {
        var user = new User { Name= userDto.Name, Email = userDto.Email };
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
