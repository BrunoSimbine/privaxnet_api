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

    [HttpPost("create"), Authorize(Roles = "admin")]
    public async Task<ActionResult<Product>> CreateProduct([FromForm]ProductDto productDto)
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

    [HttpGet("get"), Authorize]
    public async Task<ActionResult<ProductViewModel>> GetPrereoduct(Guid Id)
    {
        return Ok("djshd");
    }

    [HttpPut("update"), Authorize]
    public async Task<ActionResult<ProductViewModel>> GetPreruyeoduct(Guid Id)
    {
        return Ok("sfd");
    }

    [HttpDelete("delete/{Id}"), Authorize]
    public async Task<ActionResult<ProductViewModel>> GetPrereodusdct(Guid Id)
    {
        return Ok("sfd");
    }
}