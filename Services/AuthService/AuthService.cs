using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Services.AuthService;

public class AuthService : IAuthService
{

    private readonly DataContext _context;


    public AuthService(DataContext context)
    {
        _context = context;
    }


    public async Task<SessionViewModel> GetToken(SessionDto sessionDto)
    {
        bool userExists = await _context.Users.AnyAsync(x => x.Name == sessionDto.Name);
        if (userExists) {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == sessionDto.Name);
            var session = new SessionViewModel();
            if (VerifyPasswordHash(sessionDto.Password, user.PasswordHash, user.PasswordSalt)){
                var token = CreateToken(user);
                var isValid = IsTokenValid(token);
                var expiration = TokenExpires(token);
                session.Token = token;
                session.IsValid = isValid;
                session.Expires = expiration;
                session.UserId = user.Id;

                return session;
            } else {
                throw new UserOrPassInvalidException("Usuario ou senha invalido");
                return session;
            }
        } else {
            throw new UserOrPassInvalidException("Usuario ou senha invsalidos");
            return new SessionViewModel();
        }
    }

    public SessionViewModel VerifyToken(string token)
    {
        var isValid = IsTokenValid(token);
        var expiration = TokenExpires(token);
        var session = new SessionViewModel{ Token = token, IsValid = isValid, Expires = expiration, UserId = Guid.NewGuid() };
        return session;
    }  



    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
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

    public bool IsTokenValid(string token){
        var jwtHandler = new JwtSecurityTokenHandler();

        if(jwtHandler.CanReadToken(token)){
            var jwtToken = jwtHandler.ReadToken(token);

            var expiration = jwtToken.ValidTo;
            return DateTime.UtcNow < expiration;
        }

        return false;
    }

    public DateTime TokenExpires(string token){
        var jwtHandler = new JwtSecurityTokenHandler();

        if(jwtHandler.CanReadToken(token)){
            var jwtToken = jwtHandler.ReadToken(token);

            var expiration = jwtToken.ValidTo;
            return expiration;
        }
        return DateTime.UtcNow;
    }
}

