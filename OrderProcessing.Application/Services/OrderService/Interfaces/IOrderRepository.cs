using OrderProcessing.Domain.Aggregates.OrderAggregate;

namespace OrderProcessing.Application.Services.OrderService.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Order order,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetPagedAsync(
        int page,
        int pageSize,
        int? customerId,
        DateTime? from,
        DateTime? to,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        int? customerId,
        DateTime? from,
        DateTime? to,
        CancellationToken cancellationToken = default);
}