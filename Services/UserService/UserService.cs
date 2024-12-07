using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using System.Security.Claims;
using privaxnet_api.Models;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.AuthService;
using privaxnet_api.Services.ProductService;
using privaxnet_api.Services.MessageService;
using privaxnet_api.Services.VoucherService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace privaxnet_api.Services.UserService;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _accessor;
	private readonly DataContext _context;
    private readonly IAuthService _authService;
    private readonly IProductService _productService;
    private readonly IMessageService _messageService;

    public UserService(IHttpContextAccessor accessor, DataContext context, IAuthService authService, IProductService productService, IMessageService messageService)
    {
        _messageService = messageService;
        _accessor = accessor;
        _authService = authService;
        _context = context;
        _productService = productService;
    }

    public Guid GetId()
    {
        var id = _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid);
        return Guid.Parse(id);
    }

    public async Task<User> CreateUserAsync(UserDto userDto)
    {
		var user = new User();
		bool userExists = _context.Users.Any(x => x.Name == userDto.Name);
        bool emailExists = _context.Users.Any(x => x.Email == userDto.Email);
        bool phoneExists = _context.Users.Any(x => x.Phone == userDto.Phone);

        if (emailExists) {
            throw new EmailAlreadyExistsException("O Email ja esta sendo usado po outro usuario");
            return new User();

        } else if(phoneExists){
            throw new PhoneAlreadyExistsException("O Contacto ja esta sendo usado po outro usuario");
            return new User();

        } else if(userExists) {
            throw new UserAlreadyExistsException("Usuario ja existe!");
            return user;

        } else {

            try {
                _authService.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.Name = userDto.Name;
                user.Phone = userDto.Phone;
                user.Email = userDto.Email;

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.ClientId = GuidToBase62(Guid.NewGuid());

                
                var messageUser = new MessageUser {
                    Name = userDto.Name,
                    Phone = userDto.Phone,
                    Email = userDto.Email
                };
                

                var resultWelcome = await _messageService.SendWelcomeAsync(messageUser);

   

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                /*

                var product = await _productService.GetDefaultProductAsync();
                var voucherDto = new VoucherDto {
                    ProductId = product.Id,
                    RequestPhone = userDto.Phone 
                };

                
                var voucher = await _voucherService.CreateVoucherAsync(voucherDto);
                
                var messageVoucher = new MessageVoucher {
                    Id = voucher.Id,
                    Code = voucher.Code,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    DurationDays = product.DurationDays,
                    DataAmount = product.DataAmount,
                    UserName = user.Name,
                    RequestPhone = userDto.Phone,
                };

                var resultVoucher = _messageService.SendVoucherAsync(messageVoucher);
                */
                return user;
            } catch(HttpRequestException ex) {
                throw new InvalidWhatsAppPhoneException("Numero de whatsapp invalido!");
                return new User();
            }
            
        }
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users; // Amizade2020.z

    }


    public async Task<User> GetUserByIdAsync(Guid Id)
    {
    	var user = new User();
        bool userExists = _context.Users.Any(x => x.Id == Id);
        if (userExists) {
            user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            return user;
        }else{
        	throw new UserNotFoundException("Usuario nao encontrado!");
        	return user;
        }
    }

    public async Task<User> SetRolesAsync(Guid Id, string role)
    {
        var user = new User();
        bool userExists = _context.Users.Any(x => x.Id == Id);
        if (userExists) {
            user = _context.Users.FirstOrDefault(x => x.Id == Id);
            user.Role = role;
            await _context.SaveChangesAsync();
            return user;
        }else{
            throw new UserNotFoundException("Usuario nao encontrado!");
            return user;
        }
    }

    public User GetUserById(Guid Id)
    {
        var user = new User();
        bool userExists = _context.Users.Any(x => x.Id == Id);
        if (userExists) {
            user = _context.Users.FirstOrDefault(x => x.Id == Id);
            return user;
        }else{
            throw new UserNotFoundException("Usuario nao encontrado!");
            return user;
        }
    }

    public async Task<User> GetUserAsync()
    {
        var Id = GetId();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
        return user;
    }

    public async Task<User> UpdateUserAsync(UserUpdateDto userUpdateDto)
    {
        var Id = GetId();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

        bool emailExists = _context.Users.Any(x => x.Email == userUpdateDto.Email);
        bool phoneExists = _context.Users.Any(x => x.Phone == userUpdateDto.Phone);
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

                var result = await _messageService.SendWelcomeAsync(messageUser);
                user.Email = userUpdateDto.Email;
                user.Phone = userUpdateDto.Phone;
                await _context.SaveChangesAsync();
                return user;
            } catch (HttpRequestException ex) {
                throw new InvalidWhatsAppPhoneException("Numero de whatsapp invalido!");
                return new User();
            }

        }


    }

    public User GetUser()
    {
        var Id = GetId();
        var user = _context.Users.FirstOrDefault(x => x.Id == Id);
        return user;
    }

    public async Task<bool> RechargeAsync(Voucher voucher)
    {
        var Id = GetId();
        var user = await GetUserByIdAsync(Id);
        var pruduct = await _productService.GetProductAsync(voucher.ProductId);
        user.Expires = DateTime.Now.AddDays(pruduct.DurationDays).ToUniversalTime();
        user.DataAvaliable += pruduct.DataAmount;
        return true;
    }

    public async Task<bool> AddConsuption(long data) 
    {
        var id = GetId();
        var user = await GetUserByIdAsync(id);
        if (user.DataAvaliable >= data) {
            user.DataAvaliable -= data;
            user.DataUsed += data;
            await _context.SaveChangesAsync();
            return true;
        } else {
            user.DataUsed += user.DataAvaliable;
            user.DataAvaliable = 0;
            await _context.SaveChangesAsync();
            return false;
        }

    }


    private string GuidToBase62(Guid guid) {
        var base62Chars = "0123456789abcdefghijklmnopqestuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        byte[] bytes = guid.ToByteArray();
        long number = BitConverter.ToInt64(bytes, 0);

        if (number < 0 ) throw new ArgumentOutOfRangeException(nameof(number), "Numero deve ser positivo.");

        var result = new StringBuilder();

        do {
            int remainder = (int)(number % 62);
            result.Insert(0, base62Chars[remainder]);
            number /= 62;
        } while (number > 0);

        return result.ToString().Substring(1, 7);
    }
}

