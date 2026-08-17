using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Services.OrderService.Interfaces;
using OrderProcessing.Domain.Aggregates.OrderAggregate;
using OrderProcessing.Infrastructure.Persistence;

namespace OrderProcessing.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderProcessingDbContext _context;

    public OrderRepository(OrderProcessingDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Order>()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Set<Order>().AddAsync(order, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetPagedAsync(int page, int pageSize, int? customerId, DateTime? from, DateTime? to, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Order>().AsQueryable();

        if (customerId.HasValue)
            query = query.Where(o => o.CustomerId == customerId.Value);

        if (from.HasValue)
            query = query.Where(o => o.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(o => o.CreatedAt <= to.Value);

        query = query.OrderByDescending(o => o.CreatedAt)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize);

        var list = await query
            .Include(o => o.Items)
            .ToListAsync(cancellationToken);

        return list;
    }

    public async Task<int> CountAsync(int? customerId, DateTime? from, DateTime? to, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Order>().AsQueryable();

        if (customerId.HasValue)
            query = query.Where(o => o.CustomerId == customerId.Value);

        if (from.HasValue)
            query = query.Where(o => o.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(o => o.CreatedAt <= to.Value);

        return await query.CountAsync(cancellationToken);
    }
}