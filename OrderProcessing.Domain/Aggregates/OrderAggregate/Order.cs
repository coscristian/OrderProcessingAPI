namespace OrderProcessing.Domain.Aggregates.OrderAggregate;

public class Order
{
    public int Id { get; private set; }

    public int CustomerId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public decimal Total { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order()
    {
    }

    private Order(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentException("CustomerId must be greater than zero.", nameof(customerId));

        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
        Total = 0m;
    }

    public static Order Create(int customerId)
    {
        return new Order(customerId);
    }

    public void AddItem(OrderItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        _items.Add(item);
    }

    public void SetTotal(decimal total)
    {
        if (total < 0)
            throw new ArgumentException("Total cannot be negative.", nameof(total));

        Total = total;
    }
}