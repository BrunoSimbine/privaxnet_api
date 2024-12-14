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
[Route("v1/[controller]")]
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

    [HttpPost("create"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> CreateMyVoucher([FromQuery] Guid ProductId)
    {
        try {
            var voucher = await _voucherService.CreateVoucherAsync(ProductId);

            return Ok(voucher);
        } catch(HttpRequestException ex) {
                return BadRequest("Numero de whatsapp Invalido!");
        } catch(InsuficientBalanceException ex) {
                return BadRequest("Nao tem saldo suficiente pra fazer a compra!");
        }
    }


    [HttpPost("create/{ClientId}"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> CreateVoucher(VoucherDto voucherDto, string ClientId)
    {
        try {
            var voucher = await _voucherService.CreateVoucherAsync(voucherDto);

            return Ok(voucher);
        } catch(HttpRequestException ex) {
                return BadRequest("Numero de whatsapp Invalido!");
        } catch(InsuficientBalanceException ex) {
                return BadRequest("Nao tem saldo suficiente pra fazer a compra!");
        }
    }

    [HttpGet("get"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> GetVokjucher(Guid Id)
    {
        return Ok("");
    }

    [HttpGet("get/{Id}"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> GetVoucher(Guid Id)
    {
        try {
            var voucher = await _voucherService.GetVoucherAsync(Id);
            return Ok(voucher);
        }catch (VoucherNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Voucher nao encontrado!!"
            });
        }
    }

    [HttpGet("all"), Authorize(Roles = "admin")]
    public async Task<ActionResult<List<VoucherViewModel>>> GetVouchers()
    {
        var vouchers = await _voucherService.GetVouchersAsync();
        return Ok(vouchers);
    }



    [HttpPut("claim/{code}"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> UseVoucher(string code)
    {
        try {
            var user = await _voucherService.UseVoucherAsync(code); 
            if(user != null) {
                return Ok(user);
            }else{
                return BadRequest(new {
                    type = "error",
                    code = 400,
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


}
