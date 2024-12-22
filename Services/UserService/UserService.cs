using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using System.Security.Claims;
using privaxnet_api.Models;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.AuthService;
using privaxnet_api.Repository.ProductRepository;
using privaxnet_api.Repository.MessageRepository;
using privaxnet_api.Services.VoucherService;
using privaxnet_api.Repository.UserRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace privaxnet_api.Services.UserService;

public class UserService : IUserService
{
	private readonly DataContext _context;
    private readonly IAuthService _authService;
    private readonly IProductRepository _productRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public UserService(DataContext context, IAuthService authService, IProductRepository productRepository, IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _authService = authService;
        _productRepository = productRepository;
    }

    public async Task<User> CreateUserAsync(UserDto userDto)
    {
        bool nameExists = _userRepository.NameExists(userDto.Name);
        bool emailExists = _userRepository.EmailExists(userDto.Email);
        bool phoneExists = _userRepository.PhoneExists(userDto.Phone);

        if (emailExists) {
            throw new EmailAlreadyExistsException("O Email ja esta sendo usado po outro usuario");
            return new User();

        } else if(phoneExists){
            throw new PhoneAlreadyExistsException("O Contacto ja esta sendo usado po outro usuario");
            return new User();

        } else if(nameExists) {
            throw new UserAlreadyExistsException("Usuario ja existe!");
            return new User();

        } else {

            try {
               var messageUser = new MessageUser {
                    Name = userDto.Name,
                    Phone = userDto.Phone,
                    Email = userDto.Email
                };

                var resultWelcome = await _messageRepository.SendWelcomeAsync(messageUser);
                var user = await _userRepository.CreateUserAsync(userDto);
 
                return user;
            } catch (HttpRequestException ex) {

                throw new InvalidWhatsAppPhoneException("Numero de whatsapp invalido!");
                return new User();
            } catch (Exception ex) {

                throw new Exception(ex.Message);
                return new User();
            }

        } 
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _userRepository.GetUsersAsync();
        return users;
    }


    public async Task<User> GetUserByIdAsync(Guid Id)
    {
        bool userExists = _userRepository.UserExists(Id);
        if(userExists)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            return user;
        } else {
            throw new UserNotFoundException("Usiario nao encontrado!");
            return new User();
        }

    }

    public async Task<User> SetRolesAsync(Guid Id, string Role)
    {
        var user = new User();
        bool userExists = _userRepository.UserExists(Id);
        if (userExists) {
            user = await _userRepository.SetRolesAsync(Id, Role);
            return user;
        }else{
            throw new UserNotFoundException("Usuario nao encontrado!");
            return user;
        }
    }

    public User GetUserById(Guid Id)
    {
        var user = new User();
        bool userExists = _userRepository.UserExists(Id);
        if (userExists) {
            user = _userRepository.GetUserById(Id);
            return user;
        }else{
            throw new UserNotFoundException("Usuario nao encontrado!");
            return user;
        }
    }

    public async Task<User> GetUserAsync()
    {
        var token = _userRepository.GetCurrentToken();
        var user = await _userRepository.GetUserAsync();
        if (token != user.Token) {
            throw new TokenNotValidException("Ola Mundo!");
            return new User();
        }else{
            return user;
        }
    }

    public async Task<User> UpdateUserAsync(UserUpdateDto userUpdateDto)
    {
        var Id = _userRepository.GetId();
        var user = await _userRepository.GetUserByIdAsync(Id);

        bool emailExists = _userRepository.EmailExists(userUpdateDto.Email);
        bool phoneExists = _userRepository.PhoneExists(userUpdateDto.Phone);
        if (emailExists) {
            throw new EmailAlreadyExistsException("O Email ja esta sendo usado po outro usuario");
            return new User();
        } else if(phoneExists){
            throw new PhoneAlreadyExistsException("O Contacto ja esta sendo usado po outro usuario");
            return new User();
        } else {
            try {

                var messageUser = new MessageUser {
                    Name = user.Name,
                    Phone = userUpdateDto.Phone,
                    Email = userUpdateDto.Email
                };

                var result = await _messageRepository.SendWelcomeAsync(messageUser);
                user.Email = userUpdateDto.Email;
                user.Phone = userUpdateDto.Phone;
                await _context.SaveChangesAsync();
                return user;
            } catch (HttpRequestException ex) {
                throw new InvalidWhatsAppPhoneException("Numero de whatsapp invalido!");
                return new User();
            }

        }
        return await _userRepository.UpdateUserAsync(userUpdateDto);
    }

    public User GetUser()
    {
        return _userRepository.GetUser();
    }

    public async Task<bool> RechargeAsync(Voucher voucher)
    {
        var Id = _userRepository.GetId();
        var user = _userRepository.GetUserById(Id);

        var pruduct = _productRepository.GetProduct(voucher.ProductId);
        user.ExpirationDate = DateTime.Now.AddDays(pruduct.DurationDays);
        user.DataAvailable += pruduct.DataAmount;

        await _userRepository.SaveChangesAsync();
        return true;
    } 

    public async Task<User> AddConsuption(long data) 
    {
        var user = _userRepository.GetUser();
        if (user.DataAvailable >= data) {
            user.DataAvailable -= data;
            user.DataUsed += data;
            await _userRepository.SaveChangesAsync();
            return user;
        } else {
            user.DataUsed += user.DataAvailable;
            user.DataAvailable = 0;
            await _userRepository.SaveChangesAsync();
            throw new InsuficientDataException("Dados nao suficientes");
            return user;
        }
    }

    public async Task<User> AddBalanceAsync(decimal balance) 
    {
        var user = _userRepository.GetUser();
        user.Balance += balance;
        await _userRepository.SaveChangesAsync();
        return user;
    }

}

