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
[Route("v1/[controller]")]
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
        } catch (InvalidWhatsAppPhoneException ex) {
            return BadRequest(new {
                type = "error",
                code = 400,
                message = "Contacto de whatsapp invalido!"
            });
        } catch (Exception ex) {
            return BadRequest(new {
                type = "error",
                code = 400,
                message = $"{ex.Message}"
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
        } catch (TokenNotValidException ex) {
            return Unauthorized(new {
                type = "error",
                code = 403,
                message = "Token is not valid!"
            }); 
        }
    }

    [HttpGet("status"), Authorize]
    public async Task<ActionResult<User>> GetStatus()
    {
        return Ok();
    }

    [HttpGet("get/active"), Authorize]
    public async Task<ActionResult<User>> GetActive()
    {
        try {
            var user = await _userService.GetActives();
            return Ok(user);
        } catch (UserNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Usuario nao encontrado!"
            }); 
        } catch (TokenNotValidException ex) {
            return Unauthorized(new {
                type = "error",
                code = 403,
                message = "Token is not valid!"
            }); 
        }
    }

        [HttpGet("get/deleted"), Authorize]
    public async Task<ActionResult<User>> GetDeleted()
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
        } catch (TokenNotValidException ex) {
            return Unauthorized(new {
                type = "error",
                code = 403,
                message = "Token is not valid!"
            }); 
        }
    }

    [HttpGet("activity/verify"), Authorize]
    public async Task<ActionResult<User>> VerifyActivity()
    {
        try {
            var user = await _userService.VerifyAsync();
            return Ok(user);
        } catch (UserNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Usuario nao encontrado!"
            }); 
        } catch (TokenNotValidException ex) {
            return Unauthorized(new {
                type = "error",
                code = 403,
                message = "Token is not valid!"
            }); 
        }
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

    [HttpGet("all"), Authorize(Roles = "admin")]
    public async Task<ActionResult<List<UserViewModel>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }


    [HttpPut("upgrade/roles/{UserId}"), Authorize(Roles = "admin")]
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

    [HttpPut("downgrade/roles/{UserId}"), Authorize(Roles = "admin")]
    public async Task<ActionResult<UserViewModel>> SetPermisdfvssion([FromQuery] string Roles, Guid UserId)
    {
        return Ok("dsfik");
    }




    [HttpPut("update/contact"), Authorize]
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
        } catch (InvalidWhatsAppPhoneException ex) {
            return BadRequest(new {
                type = "error",
                code = 400,
                message = "Contacto de whatsapp invalido!"
            });
        }
    }

    [HttpPut("update/email"), Authorize]
    public async Task<ActionResult<User>> GetUsersgerById(UserUpdateDto userUpdate)
    {
        return Ok("sd");
    }


    [HttpPut("update/phone"), Authorize]
    public async Task<ActionResult<User>> GetUsersgerdfbfById(UserUpdateDto userUpdate)
    {
        return Ok("sd");
    }




    [HttpPut("update/duration"), Authorize(Roles = "admin")]
    public async Task<ActionResult<User>> AddBalance([FromQuery] long days)
    {
        try {
            var user = await _userService.AddDays(days);
            return Ok(user);
        } catch (UserNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Usuario nao encontrado!"
            }); 
        }
    }
                                                                                           


    [HttpPut("update/data/{data}"), Authorize]
    public async Task<ActionResult<User>> AddConseghuption(long data)
    {
        try {
            var user = await _userService.AddConsuption(data);
            return Ok(user);
        } catch (InsuficientDataException ex) {
            return Forbid();
        }
    }

    [HttpDelete("delete"), Authorize]
    public async Task<ActionResult<User>> UserDelete(long data)
    {
        return Ok("Ola MundoQ");
    }

    [HttpDelete("delete/{Id}"), Authorize]
    public async Task<ActionResult<User>> AddConsuption(long data)
    {
        return Ok("sf");
    }

}
