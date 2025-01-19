using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.PayAgentService;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using System.Security.Cryptography;


namespace privaxnet_api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class PayAgentController : ControllerBase
{
    private readonly IPayAgentService _payAgentService;


    public PayAgentController(IPayAgentService payAgentService)
    {
        _payAgentService = payAgentService;
    }


    [HttpPost("create"), Authorize(Roles = "admin")]
    public async Task<ActionResult<PayAgent>> Create(PayAgentDto payAgentDto)
    {
        return Ok(await _payAgentService.CreatePayAgent(payAgentDto));
    }

    [HttpGet("get/{Id}")]
    public async Task<ActionResult<PayAgent>> GetApyAgent(Guid Id)
    {
        try{

            var payAgent = await _payAgentService.GetPayAgentAsync(Id);
            return Ok(payAgent);

        } catch (PayAgentNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "PayAgent Not Found!"
            });
        }
    }

    [HttpGet("get/active")]
    public async Task<ActionResult<List<PayAgent>>> GetPayAgents()
    {
        return Ok(await _payAgentService.GetPayAgentsAsync());
    }

    [HttpGet("get/deleted")]
    public async Task<ActionResult<List<PayAgent>>> GetDeleted()
    {
        return Ok(await _payAgentService.GetDeleted());
    }


    [HttpPut("update/name"), Authorize(Roles = "admin")]
    public async Task<ActionResult<PayAgentViewModel>> UpdateName(PayAgentNameDto payAgentName)
    {
        var payAgent = await _payAgentService.UpdateName(payAgentName);
        return Ok(payAgent);
    }

    [HttpPut("update/account"), Authorize(Roles = "admin")]
    public async Task<ActionResult<PayAgentViewModel>> UpdateAccount(PayAgentAccountDto payAgentAccount)
    {
        var payAgent = await _payAgentService.UpdateAccount(payAgentAccount);
        return Ok(payAgent);
    }

    [HttpPut("recover/{Id}"), Authorize(Roles = "admin")]
    public async Task<ActionResult<PayAgentViewModel>> Recover(Guid Id)
    {
        var payAgent = await _payAgentService.Recover(Id);
        return Ok(payAgent);
    }

    [HttpDelete("delete/{Id}"), Authorize(Roles = "admin")]
    public async Task<ActionResult<SessionViewModel>> Delete(Guid Id)
    {
        var payAgent = await _payAgentService.Delete(Id);
        return Ok(payAgent);
    }


}
