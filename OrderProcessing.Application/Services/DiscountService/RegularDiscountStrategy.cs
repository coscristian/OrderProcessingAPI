using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Application.Services.Discounts;

public class RegularDiscountStrategy : IDiscountStrategy
{
    public CustomerTier Tier => CustomerTier.Regular;

    public decimal CalculateDiscount(decimal subtotal)
    {
        return 0m;
    }
}