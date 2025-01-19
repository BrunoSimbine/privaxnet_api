using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.Repository.CurrencyRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace privaxnet_api.Services.CurrencyService;

public class CurrencyService : ICurrencyService
{
	private readonly ICurrencyRepository _currencyRepository;

    public CurrencyService(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Currency> Create(CurrencyDto currency)
    {
        var labelExists = await _currencyRepository.LabelExists(currency.Label);
        var labelIdExists = await _currencyRepository.LabelIdExists(currency.LabelId);

        if (labelExists)
        {
            throw new CurrencyLabelOrSKUAlreadyExistsException("Label or SKU already exists");
            return new Currency();
        } else if (labelIdExists)
        {
            throw new CurrencyLabelOrSKUAlreadyExistsException("Label or SKU already exists");
            return new Currency();

        } else {
            return await _currencyRepository.Create(currency);
        }
    }

    public async Task<List<Currency>> GetCurrencies()
    {
        var currencies = await _currencyRepository.GetCurrencies();
        return currencies;
    }

    public async Task<List<Currency>> GetDeleted()
    {
        var currencies = await _currencyRepository.GetDeleted();
        return currencies;
    }

    public async Task<Currency> Restore(Guid Id)
    {
        var currency = await _currencyRepository.Restore(Id);
        return currency;
    }



    public async Task<Currency> GetCurrency(Guid Id)
    {
        var currencyExists = await _currencyRepository.CurrencyExists(Id);

        if(currencyExists) {
            return await _currencyRepository.GetCurrencyAsync(Id);
        }else{
            throw new CurrencyNotFoundException("Currency not found");
            return new Currency();
        }
    }

    public async Task<Currency> UpdateRate(RateDto rateDto)
    {
        var currency = await _currencyRepository.GetCurrencyAsync(rateDto.CurrencyId);
        currency.Rate = rateDto.Rate;
        return await _currencyRepository.UpdateRate(currency);
    }

    public async Task<Currency> Delete(Guid Id)
    {
        var currency = await _currencyRepository.Delete(Id);
        return currency;
    }

}

