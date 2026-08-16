using OrderProcessing.Application.Services.ProductService.Dto;
using OrderProcessing.Application.Services.ProductService.Interfaces;
using OrderProcessing.Domain.Aggregates.ProductAggregate;

namespace OrderProcessing.Application.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request)
    {
        var product = Product.Create(
            request.Name,
            request.Price,
            request.StockQuantity);

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return MapToResponse(product);
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        return product is null
            ? null
            : MapToResponse(product);
    }

    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<ProductResponse?> UpdateAsync(
        int id,
        UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return null;

        product.Update(
            request.Name,
            request.Price,
            request.StockQuantity);

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();

        return MapToResponse(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return false;

        _productRepository.Delete(product);

        await _productRepository.SaveChangesAsync();

        return true;
    }

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Price,
            product.StockQuantity);
    }
}