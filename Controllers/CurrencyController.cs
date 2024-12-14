using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.CurrencyService;
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
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;


    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpPost("create"), Authorize(Roles = "admin")]
    public async Task<ActionResult<Currency>> Create(CurrencyDto currencyDto)
    {
        try {
            var currency = await _currencyService.Create(currencyDto);
            return Ok(currency);
        } catch (CurrencyLabelOrSKUAlreadyExistsException ex) {
              return BadRequest(new {
                type = "error",
                code = 400,
                message = "The Label or SKU are alreadry used!"
            });
        }
    }

    [HttpGet("get/{Id}")]
    public async Task<ActionResult<Currency>> GsdetoiyTowedken(Guid Id)
    {
        try{

            var currency = await _currencyService.GetCurrency(Id);
            return Ok(currency);

        } catch (CurrencyNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Currency Not Found!"
            });
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<Currency>>> GetCurrencies()
    {
        return Ok(await _currencyService.GetCurrencies());
    }


    [HttpPut("update"), Authorize(Roles = "admin")]
    public async Task<ActionResult<SessionViewModel>> GsdetoiyToken()
    {
        return Ok("A caminho");
    }

    [HttpPut("update/rate/{Rate}"), Authorize(Roles = "admin")]
    public async Task<ActionResult<SessionViewModel>> GsdetoiyToken(decimal Rate)
    {
        return Ok("A caminho");
    }

    [HttpDelete("delete"), Authorize(Roles = "admin")]
    public async Task<ActionResult<SessionViewModel>> GsdetToken()
    {
        return Ok("A caminho");
    }

}
