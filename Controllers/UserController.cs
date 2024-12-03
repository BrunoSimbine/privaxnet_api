using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Exceptions;
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
        try {
            var user = await _userService.CreateUserAsync(userDto);
            return Ok(user);
        } catch (UserAlreadyExistsException ex) {
            return Conflict(new {
                type = "error",
                code = 409,
                message = "O nome de usuario ja existe!"
            });
        } catch (EmailAlreadyExistsException ex) {
            return Conflict(new {
                type = "error",
                code = 409,
                message = "O Email de usuario ja existe!"
            });
        } catch (PhoneAlreadyExistsException ex) {
            return Conflict(new {
                type = "error",
                code = 409,
                message = "O Contacto de usuario ja existe!"
            });
        }

    }

    [HttpPost("roles/set/{UserId}"), Authorize(Roles = "admin")]
    public async Task<ActionResult<UserViewModel>> SetPermission([FromQuery] string Roles, Guid UserId)
    {
        try {
            var user = await _userService.SetRolesAsync(UserId, Roles);
            return Ok(user);
        } catch (UserNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 409,
                message = "O nome de usuario ja existe!"
            });
        }
    }
    [HttpGet("all"), Authorize(Roles = "admin")]
    public async Task<ActionResult<List<UserViewModel>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("get/{Id}"), Authorize(Roles = "admin")]
    public async Task<ActionResult<User>> GetUserById(Guid Id)
    {
        try {
            var user = await _userService.GetUserByIdAsync(Id);
            return Ok(user);
        } catch (UserNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Usuario nao encontrado!"
            });
        }
    }

    [HttpPut("contact/update"), Authorize]
    public async Task<ActionResult<User>> GetUserById(UserUpdateDto userUpdate)
    {
        try {
            var user = await _userService.UpdateUserAsync(userUpdate);
            return Ok(user);
        } catch (EmailAlreadyExistsException ex) {
            return Conflict(new {
                type = "error",
                code = 409,
                message = "O Email de usuario ja existe!"
            });
        } catch (PhoneAlreadyExistsException ex) {
            return Conflict(new {
                type = "error",
                code = 409,
                message = "O Contacto de usuario ja existe!"
            });
        }
    }



    [HttpGet("get"), Authorize]
    public async Task<ActionResult<User>> GetUser()
    {
        try {
            var user = await _userService.GetUserAsync();
            return Ok(user);
        } catch (UserNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Usuario nao encontrado!"
            }); 
        }
    }
                                                                                           
    [HttpGet("consuption/{data}"), Authorize]
    public async Task<ActionResult<User>> AddConsuption(long data)
    {
        var consuption = await _userService.AddConsuption(data);
        if(consuption){
            return Ok(new {
                type = "success",
                code = 200,
                message = "Consumido com sucesso!"
            });
        }else{
            return Forbid();
        }
        return Ok(consuption);
    }

}
