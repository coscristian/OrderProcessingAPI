using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.Application.Services.CustomerService;
using OrderProcessing.Application.Services.CustomerService.Interfaces;
using OrderProcessing.Application.Services.ProductService;
using OrderProcessing.Application.Services.ProductService.Interfaces;

namespace OrderProcessing.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();
        
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        
        return services;
    }
}