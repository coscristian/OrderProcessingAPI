namespace OrderProcessing.Domain.Aggregates.OrderAggregate;

public class OrderItem
{
    public int Id { get; private set; }

    public int OrderId { get; private set; }

    public int ProductId { get; private set; }

    public int Quantity { get; private set; }

    private OrderItem()
    {
    }

    private OrderItem(int productId, int quantity)
    {
        if (productId <= 0)
            throw new ArgumentException("ProductId must be greater than zero.", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        ProductId = productId;
        Quantity = quantity;
    }

    public static OrderItem Create(int productId, int quantity)
    {
        return new OrderItem(productId, quantity);
    }
}