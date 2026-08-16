using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.Application.Services.CustomerService.Interfaces;
using OrderProcessing.Application.Services.ProductService.Interfaces;
using OrderProcessing.Infrastructure.Persistence;
using OrderProcessing.Infrastructure.Repositories;

namespace OrderProcessing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderProcessingDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}