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

namespace privaxnet_api.Repository.ProductRepository;

public class ProductRepository : IProductRepository
{
	private readonly DataContext _context;

    public ProductRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateProductAsync(ProductDto productDto)
    {
        var product = new Product {
            Name = productDto.Name,
            Price = productDto.Price,
            DataAmount = productDto.DataAmount,
            DurationDays = productDto.DurationDays
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<List<Product>> GetProducts()
    {
        var products = await _context.Products.ToListAsync();
        return products;
    }

    public async Task<Product> GetProductAsync(Guid Id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == Id);
        return product;
    }

    public Product GetProduct(Guid Id)
    {
        var product = _context.Products.FirstOrDefault(x => x.Id == Id);
        return product;
    }

    public bool ProductExists(Guid Id)
    {
        return _context.Products.Any(x => x.Id == Id);
    }

}

