using OrderProcessing.Application.Services.ProductService.Dto;

namespace OrderProcessing.Application.Services.ProductService.Interfaces;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request);

    Task<ProductResponse?> GetByIdAsync(int id);

    Task<IReadOnlyList<ProductResponse>> GetAllAsync();

    Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request);

    Task<bool> DeleteAsync(int id);
}