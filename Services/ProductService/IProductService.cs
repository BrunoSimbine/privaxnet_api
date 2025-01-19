using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Models;

namespace privaxnet_api.Services.ProductService;

public interface IProductService
{
    Task<Product> CreateProductAsync(ProductDto productDto);
    Task<List<Product>> GetProducts();
    Product GetProduct(Guid Id);
    Task<Product> UpdatePrice(ProductPriceDto productPriceDto);
    Task<Product> UpdateDuration(ProductDurationDto productDurationDto);
    Task<Product> GetProductAsync(Guid Id);
    Task<Product> Recover(Guid Id);
    Task<Product> Delete(Guid Id);
    Task<List<Product>> GetDeleted();
}