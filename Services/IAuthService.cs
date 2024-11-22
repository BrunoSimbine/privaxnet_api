using privaxnet_api.Models;

namespace privaxnet_api.Services.AuthService;

public interface IAuthService
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    string CreateToken(User user);
    DateTime TokenExpires(string token);
    bool IsTokenValid(string token);

}