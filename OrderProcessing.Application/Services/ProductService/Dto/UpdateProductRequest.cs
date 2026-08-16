namespace OrderProcessing.Application.Services.ProductService.Dto;

public record UpdateProductRequest(
    string Name,
    decimal Price,
    int StockQuantity);