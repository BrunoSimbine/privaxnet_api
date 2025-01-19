using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace privaxnet_api.Repository.CurrencyRepository;

public class CurrencyRepository : ICurrencyRepository
{
	private readonly DataContext _context;

    public CurrencyRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Currency> Create(CurrencyDto currencyDto)
    {
        var currency = new Currency {  
            Label = currencyDto.Label,
            LabelId = currencyDto.LabelId,
            Rate = currencyDto.Rate,
            Symbol = currencyDto.Symbol,
            Name = currencyDto.Name
        };

        _context.Currencies.Add(currency);
        await _context.SaveChangesAsync();
        return currency;
    }

    public async Task<List<Currency>> GetCurrencies()
    {
        var currencies = await _context.Currencies.Where(c => c.DateDeleted == null).ToListAsync();
        return currencies;
    }

    public async Task<List<Currency>> GetDeleted()
    {
        var currencies = await _context.Currencies.Where(c => c.DateDeleted != null).ToListAsync();
        return currencies;
    }

    public async Task<Currency> Restore(Guid Id)
    {
        var currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Id == Id);
        currency.DateDeleted = null;
        currency.DateUpdated = DateTime.Now;
        await _context.SaveChangesAsync();
        return currency;
    }


    public async Task<bool> LabelExists(string Label)
    {
        var labelExists = await _context.Currencies.AnyAsync(x => x.Label == Label);
        return labelExists;
    }

    public async Task<bool> LabelIdExists(string LabelId)
    {
        var labelIdExists = await _context.Currencies.AnyAsync(x => x.LabelId == LabelId);
        return labelIdExists;
    }

    public async Task<bool> CurrencyExists(Guid Id)
    {
        var currencyExists = await _context.Currencies.AnyAsync(x => x.Id == Id);
        return currencyExists;
    }

    public async Task<Currency> GetCurrencyAsync(Guid Id)
    {
        var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == Id);
        return currency;
    }

    public async Task<Currency> UpdateCurrency(Guid Id)
    {
        var currency = _context.Currencies.FirstOrDefault(x => x.Id == Id);
        currency.DateDeleted = DateTime.Now;
        await _context.SaveChangesAsync();
        return currency;
    }


    public async Task<Currency> UpdateRate(Currency currency)
    {
        var myCurrency = _context.Currencies.FirstOrDefault(x => x.Id == currency.Id);
        myCurrency.Rate = currency.Rate;
        myCurrency.DateUpdated = DateTime.Now;
        await _context.SaveChangesAsync();
        return myCurrency;
    }

    public async Task<Currency> Delete(Guid Id)
    {
        var currency = _context.Currencies.FirstOrDefault(x => x.Id == Id);
        currency.DateDeleted = DateTime.Now;
        currency.DateUpdated = DateTime.Now;

        await _context.SaveChangesAsync();
        return currency;
    }

}

