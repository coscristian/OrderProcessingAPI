namespace OrderProcessing.Application.Services.OrderService.Dto;

public record OrderItemResponse(int ProductId, int Quantity);

public record OrderResponse(
    int Id,
    int CustomerId,
    DateTime CreatedAt,
    decimal Total,
    IReadOnlyList<OrderItemResponse> Items);