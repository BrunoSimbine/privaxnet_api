using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using privaxnet_api.Models;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Exceptions;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace privaxnet_api.Services.ProductService;

public class ProductService : IProductService
{

    private readonly DataContext _context;


    public ProductService(DataContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateProductAsync(ProductDto productDto)
    {
        bool productExists = _context.Products.Any(x => x.Name == productDto.Name);
        if (!productExists) {
            var product = new Product {
                Name = productDto.Name,
                Price = productDto.Price,
                DataAmount = productDto.DataAmount,
                DurationDays = productDto.DurationDays
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        } else {
            throw new ProductAlreadyExistsException("O Produto ja existe!");
            return new Product();
        }
    }

    public async Task<List<Product>> GetProducts()
    {
        var products = await _context.Products.ToListAsync();
        return products;
    }

    public async Task<Product> GetProductAsync(Guid Id)
    {

        var product = new Product();
        bool productExists = _context.Products.Any(x => x.Id == Id);

        if (productExists) {
            product = await _context.Products.FirstOrDefaultAsync(x => x.Id == Id);
            return product;
        }else{
            throw new ProductNotFoundException("Produto nao encontrado");
            return new Product();
        }
    }

    public Product GetProduct(Guid Id)
    {

        var product = new Product();
        bool productExists = _context.Products.Any(x => x.Id == Id);

        if (productExists) {
            product = _context.Products.FirstOrDefault(x => x.Id == Id);
            return product;
        }else{
            throw new ProductNotFoundException("Produto nao encontrado");
            return new Product();
        }
    }

}

