namespace OrderProcessing.Domain.Aggregates.ProductAggregate;

public class Product
{
    public int Id { get; private set; }

    public string Name { get; private set; } = null!;

    public decimal Price { get; private set; }

    public int StockQuantity { get; private set; }
    public bool IsDeleted { get; private set; }

    private Product()
    {
        
    }

    private Product(string name, decimal price, int stockQuantity)
    {
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public static Product Create(
        string name,
        decimal price,
        int stockQuantity)
    {
        ValidateName(name);
        ValidatePrice(price);
        ValidateStock(stockQuantity);

        return new Product(name, price, stockQuantity);
    }

    public void Update(
        string name,
        decimal price,
        int stockQuantity)
    {
        ValidateName(name);
        ValidatePrice(price);
        ValidateStock(stockQuantity);

        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.",
                nameof(quantity));

        StockQuantity += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.",
                nameof(quantity));

        if (quantity > StockQuantity)
            throw new InvalidOperationException(
                "Insufficient stock.");

        StockQuantity -= quantity;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Product name is required.",
                nameof(name));
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException(
                "Product price must be greater than zero.",
                nameof(price));
    }

    private static void ValidateStock(int stockQuantity)
    {
        if (stockQuantity <= 0)
            throw new ArgumentException(
                "Stock quantity cannot be negative.",
                nameof(stockQuantity));
    }
    
    public void Delete()
    {
        IsDeleted = true;
    }
}