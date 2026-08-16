using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Services.ProductService.Interfaces;
using OrderProcessing.Domain.Aggregates.ProductAggregate;
using OrderProcessing.Infrastructure.Persistence;

namespace OrderProcessing.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly OrderProcessingDbContext _context;

    public ProductRepository(OrderProcessingDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(product => product.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
    }

    public void Delete(Product product)
    {
        product.Delete();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products
            .AnyAsync(product => product.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}