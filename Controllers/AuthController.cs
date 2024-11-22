using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
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

    public AuthController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public ActionResult GetToken(UserDto userDto)
    {
        var user = new User { Name = userDto.Name };
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "algumacoisa"),
            new Claim("UserId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jwfhgfhjsgdvjhdsg837483hf8743tfg8734gfyegf7634gf38734"));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            "bruno.com",
            "bruno.com",
            claims,
            expires: DateTime.Now.AddDays(2),
            signingCredentials: signIn
            );

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        var session = new SessionViewModel { Token = tokenValue, Status = "Active"};
        return Ok(session);


        //return Ok(user);
    }

    [HttpGet("status")]
    [Authorize]
    public ActionResult GetToken([FromQuery] string token)
    {
        var login = new SessionViewModel();
        return Ok(login);


    }  


}
