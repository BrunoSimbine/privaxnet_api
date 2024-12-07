using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.VoucherService;
using privaxnet_api.Services.UserService;
using privaxnet_api.Services.ProductService;
using privaxnet_api.Services.MessageService;
using Microsoft.AspNetCore.Authorization;



namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class VoucherController : ControllerBase
{
    private readonly IVoucherService _voucherService;
    private readonly IProductService _productService;
    private readonly IMessageService _messageService;



    public VoucherController(IVoucherService voucherService, IProductService productService, IMessageService messageService)
    {
        _voucherService = voucherService;
        _productService = productService;
        _messageService = messageService;

    }

    [HttpPost("register"), Authorize(Roles = "admin")]
    public async Task<ActionResult<VoucherViewModel>> CreateVoucher(VoucherDto voucherDto)
    {
        try {
            var voucher = await _voucherService.CreateVoucherAsync(voucherDto);
            var product = await _productService.GetProductAsync(voucherDto.ProductId);

            var messageVoucher = new MessageVoucher {
                Code = voucher.Code,
                ProductName = product.Name,
                ProductPrice = product.Price,
                DurationDays = product.DurationDays,
                DataAmount = product.DataAmount,
                RequestPhone = voucherDto.RequestPhone
            };

            var resultVoucher = await _messageService.SendVoucherAsync(messageVoucher);
            return Ok(voucher);
        } catch(HttpRequestException ex) {
                return BadRequest("Numero de whatsapp Invalido!");
            }

    }

    [HttpPost("use/{code}"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> UseVoucher(string code)
    {
        try {
            var used = await _voucherService.UseVoucherAsync(code); 
            if(used) {
                return Ok(new {
                    type = "success",
                    code = 200,
                    message = "Usado com sucesso"
                });
            }else{
                return BadRequest(new {
                    type = "error",
                    code = 500,
                    message = "Impossivel recarregar"
                });
            }

        } catch (VoucherAlreadyUsedException ex) {
            return Conflict(new {
                type = "error",
                code = 409,
                message = "Voucher ja foi usado!"
            });
        } catch (VoucherNotFoundException ex ) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Voucher nao existe!"
            });

        }
    }

    [HttpGet("all"), Authorize(Roles = "admin")]
    public async Task<ActionResult<List<VoucherViewModel>>> GetVouchers()
    {
        var vouchers = await _voucherService.GetVouchersAsync();
        return Ok(vouchers);
    }

    [HttpPost("get/{Id}"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> GetVoucher(Guid Id)
    {
        try {
            var voucher = _voucherService.GetVoucherAsync(Id);
            return Ok(voucher);
        }catch (VoucherNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Voucher nao encontrado!!"
            });
        }
    }
}
