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

    [HttpGet("all")]
    public async Task<ActionResult<List<PayAgent>>> GetPayAgents()
    {
        return Ok(await _payAgentService.GetPayAgentsAsync());
    }


    [HttpPut("update"), Authorize(Roles = "admin")]
    public async Task<ActionResult<SessionViewModel>> GsdetoiyToken()
    {
        return Ok("A caminho");
    }


    [HttpDelete("delete"), Authorize(Roles = "admin")]
    public async Task<ActionResult<SessionViewModel>> GsdetToken()
    {
        return Ok("A caminho");
    }


}
