using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.Repository.ProductRepository;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Services.ProductService;

public class ProductService : IProductService
{

    private readonly IProductRepository _productRepository;
    private readonly DataContext _context;


    public ProductService(IProductRepository productRepository, DataContext context)
    {
        _context = context;
        _productRepository = productRepository;
    }

    public async Task<Product> CreateProductAsync(ProductDto productDto)
    {
        return await _productRepository.CreateProductAsync(productDto);
    }

    public async Task<List<Product>> GetProducts()
    {
        return await _productRepository.GetProducts();
    }

    public async Task<List<Product>> GetDeleted()
    {
        return await _productRepository.GetDeleted();
    }

    public async Task<Product> UpdatePrice(ProductPriceDto productPriceDto)
    {
        return await _productRepository.UpdatePrice(productPriceDto);
    }

    public async Task<Product> UpdateDuration(ProductDurationDto productDurationDto)
    {
        return await _productRepository.UpdateDuration(productDurationDto);
    }

    public async Task<Product> GetProductAsync(Guid Id)
    {
        return await _productRepository.GetProductAsync(Id);
    }

    public Product GetProduct(Guid Id)
    {
        return _productRepository.GetProduct(Id);
    }

    public async Task<Product> Recover(Guid Id)
    {
        return await _productRepository.Recover(Id);
    }

    public async Task<Product> Delete(Guid Id)
    {
        return await _productRepository.Delete(Id);
    }

}

