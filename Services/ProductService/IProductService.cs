using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.ProductService;

public interface IProductService
{
    Task<Product> CreateProductAsync(ProductDto productDto);
    Task<List<Product>> GetProducts();
    Product GetProduct(Guid Id);
    Task<Product> GetProductAsync(Guid Id);
}