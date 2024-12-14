using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;



namespace privaxnet_api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create"), Authorize]
    public async Task<ActionResult<Payment>> CreatePayment(PaymentDto paymentDto)
    {
        return Ok(await _paymentService.CreatePayment(paymentDto));
    }

    [HttpGet("get/{Id}"), Authorize]
    public async Task<ActionResult<ProductViewModel>> GetProduct(Guid Id)
    {
        return Ok("A caminho!");
    }

    [HttpGet("get"), Authorize]
    public async Task<ActionResult<ProductViewModel>> GetMyPayments(Guid Id)
    {
        return Ok("A caminho!");
    }

    [HttpGet("all"), Authorize]
    public async Task<ActionResult<List<Payment>>> GetPayments()
    {
        return Ok(await _paymentService.GetPayments());
    }

    [HttpPut("update"), Authorize]
    public async Task<ActionResult<ProductViewModel>> GetPayuytments()
    {
        return Ok("A caminho!");
    }

    [HttpPut("aprove/{Id}"), Authorize(Roles = "admin")]
    public async Task<ActionResult<User>> AprovePayment(Guid Id)
    {
        return Ok(await _paymentService.AprovePayment(Id));
    }

    [HttpDelete("delete/{Id}"), Authorize]
    public async Task<ActionResult<ProductViewModel>> GetPayuytments(Guid Id)
    {
        return Ok("A caminho!");
    }
}