namespace OrderProcessing.Application.Services.OrderService.Dto;

public record PagedOrdersResponse(
    IReadOnlyList<OrderResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);