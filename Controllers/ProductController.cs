using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;
using privaxnet_api.Exceptions;
using privaxnet_api.Services.ProductService;
using Microsoft.AspNetCore.Authorization;

namespace privaxnet_api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("create"), Authorize]
    public async Task<ActionResult<Product>> CreateProduct(ProductDto productDto)
    {
        try {
            var product = await _productService.CreateProductAsync(productDto);
            return Ok(product);
        } catch (ProductAlreadyExistsException ex) {
            return Conflict(new {
                type = "error",
                code = 409,
                message = "O produto ja existe!"
            });
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<ProductViewModel>>> GetProducts()
    {
        var products = await _productService.GetProducts();
        return Ok(products);
    }

    [HttpGet("get/{Id}"), Authorize]
    public async Task<ActionResult<ProductViewModel>> GetProduct(Guid Id)
    {
        try {
            var product = await _productService.GetProductAsync(Id);
            return Ok(product);
        } catch (ProductNotFoundException ex) {
            return NotFound(new {
                type = "error",
                code = 404,
                message = "Produto nao encontrado!!"
            });
        }

    }

    [HttpGet("get/deleted"), Authorize]
    public async Task<ActionResult<ProductViewModel>> GetDeleted()
    {
        var products = await _productService.GetDeleted();
        return Ok(products);
    }

    [HttpPut("update/price"), Authorize]
    public async Task<ActionResult<Product>> UpdatePrice(ProductPriceDto productPriceDto)
    {
        var product = await _productService.UpdatePrice(productPriceDto);
        return Ok(product);
    }

    [HttpPut("update/duration"), Authorize]
    public async Task<ActionResult<Product>> UpdateDuration(ProductDurationDto productDurationDto)
    {
        var product = await _productService.UpdateDuration(productDurationDto);
        return Ok(product);
    }

    [HttpPut("update/recover/{Id}"), Authorize]
    public async Task<ActionResult<Product>> Recover(Guid Id)
    {
        var product = await _productService.Recover(Id);
        return Ok(product);
    }


    [HttpDelete("delete/{Id}"), Authorize]
    public async Task<ActionResult<Product>> Delete(Guid Id)
    {
        var product = await _productService.Delete(Id);
        return Ok(product);
    }
}