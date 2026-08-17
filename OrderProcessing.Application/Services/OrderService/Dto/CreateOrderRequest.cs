namespace OrderProcessing.Application.Services.OrderService.Dto;

public record OrderItemRequest(int ProductId, int Quantity);

public record CreateOrderRequest(int CustomerId, IReadOnlyList<OrderItemRequest> Items);