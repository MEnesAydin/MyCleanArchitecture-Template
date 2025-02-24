using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanArchitectureTemplate.Domain.Products;


namespace CleanArchitectureTemplate.Infrastructure.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Price).HasColumnType("money");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
