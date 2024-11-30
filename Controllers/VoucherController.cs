using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.VoucherService;
using Microsoft.AspNetCore.Authorization;



namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class VoucherController : ControllerBase
{
    private readonly IVoucherService _voucherService;


    public VoucherController(IVoucherService voucherService)
    {
        _voucherService = voucherService;
    }

    [HttpPost("register"), Authorize(Roles = "admin")]
    public async Task<ActionResult<VoucherViewModel>> CreateVoucher(VoucherDto voucherDto)
    {
        var voucher = await _voucherService.CreateVoucherAsync(voucherDto);
        return Ok(voucher);
    }

    [HttpPost("use/{code}"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> UseVoucher(string code)
    {
        try {
            var used = _voucherService.UseVoucherAsync(code); 
            return Ok(new {
                type = "success",
                code = 200,
                message = "Usado com sucesso"
            });
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
