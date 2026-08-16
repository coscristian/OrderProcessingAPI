using OrderProcessing.Domain.Aggregates.ProductAggregate;

namespace OrderProcessing.Application.Services.ProductService.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);

    Task<IReadOnlyList<Product>> GetAllAsync();

    Task AddAsync(Product product);

    void Update(Product product);

    void Delete(Product product);

    Task<bool> ExistsAsync(int id);

    Task SaveChangesAsync();
}