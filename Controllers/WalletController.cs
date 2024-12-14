using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.WalletService;
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
[Route("v1/[controller]"), Authorize]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;


    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Wallet>> CreateWallet(WalletDto walletDto)
    {
        var wallet = await _walletService.CreateWallet(walletDto);
        return Ok(wallet);
    }

    [HttpGet("get/{Id}")]
    public async Task<ActionResult<SessionViewModel>> GsdetoiyTowedken(Guid Id)
    {


        try {
            var wallet = await _walletService.GetWallet(Id);
            return Ok(wallet);
        } catch (WalletNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Wallet Not Found!"
            }); 
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<Wallet>>> GetWallets()
    {
        var wallets = await _walletService.GetWallets();
        return Ok(wallets);
    }


    [HttpPut("update")]
    public async Task<ActionResult<SessionViewModel>> GsdetoiyToken(SessionDto sessionDto)
    {
        return Ok("A caminho");
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<SessionViewModel>> GsdetToken(SessionDto sessionDto)
    {
        return Ok("A caminho");
    }

}
