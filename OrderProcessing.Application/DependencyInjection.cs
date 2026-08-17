using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.Application.Services.CustomerService;
using OrderProcessing.Application.Services.CustomerService.Interfaces;
using OrderProcessing.Application.Services.ProductService;
using OrderProcessing.Application.Services.ProductService.Interfaces;
using OrderProcessing.Application.Services.OrderService;
using OrderProcessing.Application.Services.Discounts;

namespace OrderProcessing.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();
        
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();

        services.AddScoped<DiscountService>();
        services.AddScoped<IDiscountStrategy, RegularDiscountStrategy>();
        services.AddScoped<IDiscountStrategy, PremiumDiscountStrategy>();
        services.AddScoped<IDiscountStrategy, VipDiscountStrategy>();
        
        return services;
    }
}