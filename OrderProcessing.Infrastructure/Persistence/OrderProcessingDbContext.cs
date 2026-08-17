using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Aggregates.CustomerAggregate;
using OrderProcessing.Domain.Aggregates.ProductAggregate;

namespace OrderProcessing.Infrastructure.Persistence;

public class OrderProcessingDbContext : DbContext
{
    public OrderProcessingDbContext(
        DbContextOptions<OrderProcessingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<OrderProcessing.Domain.Aggregates.OrderAggregate.Order> Orders => Set<OrderProcessing.Domain.Aggregates.OrderAggregate.Order>();
    public DbSet<OrderProcessing.Domain.Aggregates.OrderAggregate.OrderItem> OrderItems => Set<OrderProcessing.Domain.Aggregates.OrderAggregate.OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(OrderProcessingDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
    
}