using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanArchitectureTemplate.Domain.Categories;

namespace CleanArchitectureTemplate.Infrastructure.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Name).HasColumnType("varchar(50)");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

