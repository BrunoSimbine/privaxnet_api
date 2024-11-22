using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using System.Security.Cryptography;


namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IAuthService _authService;


    public AuthController(DataContext context, IAuthService authService)
    {
        _authService = authService;
        _context = context;
    }

    [HttpPost("login")]
    public ActionResult<SessionViewModel> GetToken(UserDto userDto)
    {
        var user = new User { Name = userDto.Name };
        var token = _authService.CreateToken(user);
        var isValid = _authService.IsTokenValid(token);
        var expiration = _authService.TokenExpires(token);
        var session = new SessionViewModel{ Token = token, IsValid = isValid, Expires = expiration };
        return Ok(session);
    }

    [HttpGet("status")]
    public ActionResult GetTxoken([FromQuery] string token)
    {
        var login = new SessionViewModel();
        return Ok(login);


    }  

}
