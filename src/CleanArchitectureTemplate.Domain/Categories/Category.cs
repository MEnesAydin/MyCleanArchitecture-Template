using CleanArhictecture_2025.Domain.Abstractions;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.Domain.Categories;

public sealed class Category : Entity
{
    public string Name { get; set; } = default!;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
