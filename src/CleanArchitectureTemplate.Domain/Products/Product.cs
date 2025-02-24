using CleanArhictecture_2025.Domain.Abstractions;
using CleanArchitectureTemplate.Domain.Categories;

namespace CleanArchitectureTemplate.Domain.Products;

public sealed class Product : Entity
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; } = default!;

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}
