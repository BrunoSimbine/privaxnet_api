using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
using privaxnet_api.Services.UserService;
using privaxnet_api.Repository.UserRepository;
using privaxnet_api.Repository.VoucherRepository;
using privaxnet_api.Repository.ProductRepository;
using privaxnet_api.Repository.MessageRepository;
using privaxnet_api.Services.ProductService;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Services.VoucherService;

public class VoucherService : IVoucherService
{
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IVoucherRepository _voucherRepository;

    public VoucherService(IUserRepository userRepository, IVoucherRepository voucherRepository, IProductRepository productRepository, IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
        _productRepository = productRepository;
        _voucherRepository = voucherRepository;
        _userRepository = userRepository;
    }

    public async Task<Voucher> CreateVoucherAsync(VoucherDto voucherDto)
    {
        var agent = _userRepository.GetUser();
        bool productExists = _productRepository.ProductExists(voucherDto.ProductId);
        if (productExists) {
            var product = _productRepository.GetProduct(voucherDto.ProductId);

            bool suficientBalance = agent.Balance >= product.Price ? true : false;

            if(suficientBalance) {
                var voucher = new Voucher { Product = product, Agent = agent, RequestPhone = voucherDto.RequestPhone };
                voucher.Code = _voucherRepository.GenerateCode();
                agent.Balance -= product.Price;

                var messageVoucher = new MessageVoucher {
                    Code = voucher.Code,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    DurationDays = product.DurationDays,
                    DataAmount = product.DataAmount,
                    RequestPhone = voucherDto.RequestPhone
                };
                await _voucherRepository.CreateVoucherAsync(voucher);
                var resultVoucher = await _messageRepository.SendVoucherAsync(messageVoucher);

                return voucher;
            } else {
                throw new InsuficientBalanceException("Produto nao encotrado!");
                return new Voucher();
            }

        } else {
            throw new ProductNotFoundException("Produto nao encotrado!");
            return new Voucher();
        }
    }

    public async Task<Voucher> CreateVoucherAsync(Guid ProductId)
    {
        var agent = _userRepository.GetUser();
        bool productExists = _productRepository.ProductExists(ProductId);
        if (productExists) {
            var product = _productRepository.GetProduct(ProductId);

            bool suficientBalance = agent.Balance >= product.Price ? true : false;
            if(suficientBalance) {
                var voucher = new Voucher { Product = product, Agent = agent, RequestPhone = agent.Phone };
                voucher.Code = _voucherRepository.GenerateCode();
                agent.Balance -= product.Price;

                var messageVoucher = new MessageVoucher {
                    Code = voucher.Code,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    DurationDays = product.DurationDays,
                    DataAmount = product.DataAmount,
                    RequestPhone = agent.Phone
                };

                await _voucherRepository.CreateVoucherAsync(voucher);
                var resultVoucher = await _messageRepository.SendVoucherAsync(messageVoucher);
                return voucher;
                
            } else {
                throw new InsuficientBalanceException("Produto nao encotrado!");
                return new Voucher();
            }

        } else {
            throw new ProductNotFoundException("Produto nao encotrado!");
            return new Voucher();
        }
    }

    public async Task<User> UseVoucherAsync(string Code)
    {
        Code = Code.Replace(" ", "");
        var voucherExists = _voucherRepository.VoucherExists(Code);
        if (voucherExists) {
            var voucher = _voucherRepository.GetVoucherByCode(Code);
            if(voucher.IsUsed == false){
                var product = await _productRepository.GetProductAsync(voucher.ProductId);
                var user = await _userRepository.RechargeAsync(product.DataAmount, product.DurationDays);
                if(user != null) {
                    voucher.IsUsed = true;
                    voucher.UserId = _userRepository.GetId();
                    await _voucherRepository.SaveChangesAsync();
                    return user;
                }
                throw new VoucherAlreadyUsedException("");
                return new User();
            } else {
                throw new VoucherAlreadyUsedException("");
                return new User();
            }
        } else {
            throw new VoucherNotFoundException("");
            return new User();
        }
    }

    public async Task<List<VoucherViewModel>> GetVouchersAsync()
    {
        return await _voucherRepository.GetVouchersAsync();
    }

    public async Task<VoucherViewModel> GetVoucherAsync(Guid Id)
    {
        return await _voucherRepository.GetVoucherAsync(Id);
    }

    public async Task<Voucher> GetVoucherByCodeAsync(string Code)
    {
        return await _voucherRepository.GetVoucherByCodeAsync(Code);
    }

    public Voucher GetVoucherByCode(string Code)
    {
        return _voucherRepository.GetVoucherByCode(Code);
    }

}