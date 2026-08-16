using FluentValidation;

namespace OrderProcessing.Application.Validators.Orders;

// public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
// {
//     public OrderItemRequestValidator()
//     {
//         RuleFor(x => x.ProductId)
//             .GreaterThan(0);
//
//         RuleFor(x => x.Quantity)
//             .GreaterThan(0);
//     }
// }