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

    [HttpGet("get/deleted")]
    public async Task<ActionResult<List<Currency>>> GetDeleted()
    {
        return Ok(await _currencyService.GetDeleted());
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<Currency>>> GetCurrencies()
    {
        return Ok(await _currencyService.GetCurrencies());
    }

    [HttpPut("restore/{Id}"), Authorize(Roles = "admin")]
    public async Task<ActionResult<Currency>> Restore(Guid Id)
    {
        var currency = await _currencyService.Restore(Id);
        return Ok(currency);
    }

    [HttpPut("update/rate"), Authorize(Roles = "admin")]
    public async Task<ActionResult<Currency>> UpdateRate(RateDto rateDto)
    {
        var currency = await _currencyService.UpdateRate(rateDto);
        return Ok(currency);
    }

    [HttpDelete("delete/{Id}"), Authorize(Roles = "admin")]
    public async Task<ActionResult<Currency>> DeleteToken(Guid Id)
    {
        var currency = await _currencyService.Delete(Id);
        return Ok(currency);
    }

}
