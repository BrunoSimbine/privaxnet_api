using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.CurrencyService;

public interface ICurrencyService
{
    Task<Currency> Create(CurrencyDto currency);
    Task<List<Currency>> GetCurrencies();
    Task<Currency> GetCurrency(Guid Id);
}