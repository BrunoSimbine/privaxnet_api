using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.ViewModels;
using privaxnet_api.Repository.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;


    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<SessionViewModel> GetToken(SessionDto sessionDto)
    {
        bool userExists = _userRepository.NameExists(sessionDto.Name);
        if (userExists) {
            var user = await _userRepository.GetUserByNameAsync(sessionDto.Name);
            var session = new SessionViewModel();
            if (_userRepository.VerifyPasswordHash(sessionDto.Password, user.PasswordHash, user.PasswordSalt)){
                var token = _userRepository.CreateToken(user);
                var isValid = _userRepository.IsTokenValid(token);
                var expiration = _userRepository.TokenExpires(token);
                session.Token = token;
                session.IsValid = isValid;
                session.Expires = expiration;
                session.UserId = user.Id;
                await _userRepository.UpdateToken(user.Id, token);

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
}
