namespace OrderProcessing.Domain.Aggregates.CustomerAggregate;

public class Customer
{
    public int Id { get; private set; }

    public string Name { get; private set; } = null!;

    public CustomerTier Tier { get; private set; }

    private Customer()
    {
    }

    private Customer(string name, CustomerTier tier)
    {
        if (!Enum.IsDefined(tier))
            throw new ArgumentException("Invalid customer tier.", nameof(tier));

        Name = name;
        Tier = tier;
    }

    public static Customer Create(
        string name,
        CustomerTier tier)
    {
        return new Customer(name, tier);
    }

    public void Update(
        string name,
        CustomerTier tier)
    {
        Name = name;
        Tier = tier;
    }
}