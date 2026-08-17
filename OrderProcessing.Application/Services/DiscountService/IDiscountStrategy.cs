using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Application.Services.Discounts;

public interface IDiscountStrategy
{
    CustomerTier Tier { get; }

    decimal CalculateDiscount(decimal subtotal);
}