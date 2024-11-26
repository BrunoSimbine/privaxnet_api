using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.ProductService;

public interface IProductService
{
    Task<Product> CreateProduct(ProductDto productDto);
    Task<List<Product>> GetProducts();
    Task<Product> GetProduct(Guid Id);
}