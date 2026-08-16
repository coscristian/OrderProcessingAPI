

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Aggregates.ProductAggregate;

namespace OrderProcessing.Infrastructure.Config;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id)
            .ValueGeneratedOnAdd();

        builder.Property(product => product.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(product => product.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(product => product.StockQuantity)
            .IsRequired();
        
        builder.Property(product => product.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.HasQueryFilter(product => !product.IsDeleted);
    }
}