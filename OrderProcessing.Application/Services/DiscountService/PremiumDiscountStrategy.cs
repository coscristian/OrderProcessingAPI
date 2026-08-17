using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Application.Services.Discounts;

public class PremiumDiscountStrategy : IDiscountStrategy
{
    public CustomerTier Tier => CustomerTier.Premium;

    public decimal CalculateDiscount(decimal subtotal)
    {
        return Math.Round(subtotal * 0.10m, 2);
    }
}