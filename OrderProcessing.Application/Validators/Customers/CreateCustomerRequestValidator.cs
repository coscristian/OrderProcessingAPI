using FluentValidation;
using OrderProcessing.Application.Services.CustomerService.Dto;

namespace OrderProcessing.Application.Validators.Customers;

public class CreateCustomerRequestValidator
    : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Tier)
            .IsInEnum();
    }
}