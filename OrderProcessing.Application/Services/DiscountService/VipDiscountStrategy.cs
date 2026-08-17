using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Application.Services.Discounts;

public class VipDiscountStrategy : IDiscountStrategy
{
    public CustomerTier Tier => CustomerTier.VIP;

    public decimal CalculateDiscount(decimal subtotal)
    {
        return Math.Round(subtotal * 0.20m, 2);
    }
}