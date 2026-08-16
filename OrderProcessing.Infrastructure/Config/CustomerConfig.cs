using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Aggregates.CustomerAggregate;

namespace OrderProcessing.Infrastructure.Config;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.Id)
            .ValueGeneratedOnAdd();

        builder.Property(customer => customer.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(customer => customer.Tier)
            .IsRequired()
            .HasConversion<int>();
    }
}