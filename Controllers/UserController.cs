using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text;



namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{    
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("create")]
    public async Task<ActionResult<UserViewModel>> CreateUser(UserDto userDto)
    {
        var user = await _userService.CreateUser(userDto);
        return Ok(user);
    }

    [HttpGet("all"), Authorize]
    public async Task<ActionResult<List<UserViewModel>>> GetUsers()
    {

        var users = await _userService.GetUsers();
        return Ok(users);

    }

    [HttpGet("get/{Id}"), Authorize]
    public async Task<ActionResult<User>> GetUserById(Guid Id)
    {
        var user = await _userService.GetUserById(Id);
        return Ok(user);
    }

}
