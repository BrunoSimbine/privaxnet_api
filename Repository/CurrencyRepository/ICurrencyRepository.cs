using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Repository.CurrencyRepository;

public interface ICurrencyRepository
{
	Task<Currency> Create(CurrencyDto currency);
	Task<List<Currency>> GetCurrencies();
	Task<List<Currency>> GetDeleted();
	Task<Currency> Restore(Guid Id);
	Task<bool> LabelIdExists(string LabelId);
	Task<bool> LabelExists(string Label);
	Task<Currency> UpdateRate(Currency currency);
	Task<Currency> GetCurrencyAsync(Guid Id);
	Task<bool> CurrencyExists(Guid Id);
	Task<Currency> Delete(Guid Id);
}