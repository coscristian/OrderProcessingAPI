namespace OrderProcessing.Application.Services.ProductService.Dto;

public record CreateProductRequest(
    string Name,
    decimal Price,
    int StockQuantity);