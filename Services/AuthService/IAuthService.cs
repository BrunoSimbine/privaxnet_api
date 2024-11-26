using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.AuthService;

public interface IAuthService
{
    Task<SessionViewModel> GetToken(SessionDto sessionDto);
    SessionViewModel VerifyToken(string token);
    string CreateToken(User user);
    DateTime TokenExpires(string token);
    bool IsTokenValid(string token);
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
}