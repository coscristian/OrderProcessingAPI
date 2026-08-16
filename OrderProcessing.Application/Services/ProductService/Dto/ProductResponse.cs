namespace OrderProcessing.Application.Services.ProductService.Dto;

public record ProductResponse(
    int Id,
    string Name,
    decimal Price,
    int StockQuantity);