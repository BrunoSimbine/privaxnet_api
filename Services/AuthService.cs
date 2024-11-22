using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;

namespace privaxnet_api.Services.AuthService;

public class AuthService : IAuthService
{
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

