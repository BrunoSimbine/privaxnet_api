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
            DataAmount = 20,
            DurationDays = productDto.DurationDays
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<List<Product>> GetProducts()
    {
        var products = await _context.Products.Where(p => p.DateDeleted == null).ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetDeleted()
    {
        var now = DateTime.Now;
        var products = await _context.Products.Where(p => p.DateDeleted <= now).ToListAsync();
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

    public async Task<Product> UpdatePrice(ProductPriceDto productPriceDto)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productPriceDto.ProductId);
        product.Price = productPriceDto.Price;
        product.DateUpdated = DateTime.Now;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateDuration(ProductDurationDto productDurationDto)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productDurationDto.ProductId);
        product.DurationDays = productDurationDto.Duration;
        product.DateUpdated = DateTime.Now;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Recover(Guid Id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
        product.DateDeleted = null;
        product.DateUpdated = DateTime.Now;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Delete(Guid Id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
        product.DateDeleted = DateTime.Now;
        product.DateUpdated = DateTime.Now;
        await _context.SaveChangesAsync();
        return product;
    }

}

