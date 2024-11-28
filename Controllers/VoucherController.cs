using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
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

    [HttpPost("register"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> CreateVoucher(VoucherDto voucherDto)
    {
        var voucher = await _voucherService.CreateVoucher(voucherDto);
        return Ok(voucher);
    }

    [HttpPost("use/{code}"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> UseVoucher(string code)
    {
        var used = _voucherService.UseVoucher(code);
        return Ok(used);
    }

    [HttpGet("all"), Authorize]
    public async Task<ActionResult<List<VoucherViewModel>>> GetVouchers()
    {
        var vouchers = await _voucherService.GetVouchers();
        return Ok(vouchers);
    }

    [HttpPost("get/{Id}"), Authorize]
    public async Task<ActionResult<VoucherViewModel>> GetVoucher(Guid Id)
    {
        return Ok("obter usuario especifico");
    }


}
