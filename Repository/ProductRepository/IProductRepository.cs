using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Repository.ProductRepository;

public interface IProductRepository
{
	Task<Product> CreateProductAsync(ProductDto productDto);
	Task<List<Product>> GetProducts();
	Task<Product> GetProductAsync(Guid Id);
	Product GetProduct(Guid Id);
	bool ProductExists(Guid Id);
}