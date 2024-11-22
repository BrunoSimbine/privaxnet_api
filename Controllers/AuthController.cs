using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ActionResult<SessionViewModel>> GetToken(SessionDto sessionDto)
    {

        try {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == sessionDto.Name);

            if (_authService.VerifyPasswordHash(sessionDto.Password, user.PasswordHash, user.PasswordSalt)){
                var token = _authService.CreateToken(user);
                var isValid = _authService.IsTokenValid(token);
                var expiration = _authService.TokenExpires(token);
                var session = new SessionViewModel{ Token = token, IsValid = isValid, Expires = expiration };
                return Ok(session);
            }

            return BadRequest("Nome de usuario ou senha invalido");

        } catch (Exception ex ) {
            return BadRequest("Nome de usuario ou senha invalido");
        }

    }

    [HttpGet("status")]
    public ActionResult VerifyToken([FromQuery] string token)
    {
        var isValid = _authService.IsTokenValid(token);
        var expiration = _authService.TokenExpires(token);
        var session = new SessionViewModel{ Token = token, IsValid = isValid, Expires = expiration };
        return Ok(session);

    }  

}
