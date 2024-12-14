using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.AuthService;

public interface IAuthService
{
    Task<SessionViewModel> GetToken(SessionDto sessionDto);
}