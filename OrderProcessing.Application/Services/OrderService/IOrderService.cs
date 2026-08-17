using OrderProcessing.Application.Services.OrderService.Dto;

namespace OrderProcessing.Application.Services.OrderService;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);

    Task<OrderResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<PagedOrdersResponse> GetPagedAsync(int page, int pageSize, int? customerId, DateTime? from, DateTime? to, CancellationToken cancellationToken = default);
}