using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Application.Services.Discounts;

public class DiscountService
{
    private readonly IEnumerable<IDiscountStrategy> _strategies;

    public DiscountService(IEnumerable<IDiscountStrategy> strategies)
    {
        _strategies = strategies;
    }

    public decimal CalculateDiscount(CustomerTier tier, decimal subtotal)
    {
        var strategy = _strategies.FirstOrDefault(s => s.Tier == tier);

        if (strategy is null)
            return 0m;

        return strategy.CalculateDiscount(subtotal);
    }
}