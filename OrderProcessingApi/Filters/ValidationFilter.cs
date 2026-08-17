using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrderProcessingApi.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var validatorType = typeof(IValidator<>)
                .MakeGenericType(argument.GetType());

            if (_serviceProvider.GetService(validatorType)
                is not IValidator validator)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(argument);

            var result = await validator.ValidateAsync(
                validationContext,
                context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(e => e.ErrorMessage).ToArray());

                var validationProblemDetails = new ValidationProblemDetails(errors)
                {
                    Type = "https://httpstatuses.com/400",
                    Title = "Validation failed",
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(validationProblemDetails);

                return;
            }
        }

        await next();
    }
}