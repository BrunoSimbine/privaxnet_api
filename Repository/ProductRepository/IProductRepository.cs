using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Repository.ProductRepository;

public interface IProductRepository
{
	Task<Product> CreateProductAsync(ProductDto productDto);
	Task<List<Product>> GetProducts();
	Task<Product> UpdatePrice(ProductPriceDto productPriceDto);
	Task<Product> UpdateDuration(ProductDurationDto productDurationDto);
	Task<Product> GetProductAsync(Guid Id);
	Product GetProduct(Guid Id);
	bool ProductExists(Guid Id);
	Task<Product> Recover(Guid Id);
	Task<Product> Delete(Guid Id);
	Task<List<Product>> GetDeleted();
}