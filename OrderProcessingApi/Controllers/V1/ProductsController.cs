using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Services.ProductService.Dto;
using OrderProcessing.Application.Services.ProductService.Interfaces;

namespace OrderProcessingApi.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(
        CreateProductRequest request)
    {
        var product = await _productService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResponse>> Update(
        int id,
        UpdateProductRequest request)
    {
        var product = await _productService.UpdateAsync(id, request);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}