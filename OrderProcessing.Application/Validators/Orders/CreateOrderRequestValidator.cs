using FluentValidation;

namespace OrderProcessing.Application.Validators.Orders;

// public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
// {
//     public CreateOrderRequestValidator()
//     {
//         RuleFor(x => x.CustomerId)
//             .GreaterThan(0);
//
//         RuleFor(x => x.Items)
//             .NotEmpty()
//             .WithMessage("An order must contain at least one item.");
//
//         RuleForEach(x => x.Items)
//             .SetValidator(new OrderItemRequestValidator());
//     }
// }