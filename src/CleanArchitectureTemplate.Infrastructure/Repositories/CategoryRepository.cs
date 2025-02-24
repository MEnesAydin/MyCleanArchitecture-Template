using CleanArhictecture_2025.Infrastructure.Context;
using GenericRepository;
using CleanArchitectureTemplate.Domain.Categories;

namespace CleanArchitectureTemplate.Infrastructure.Repositories;

internal sealed class CategoryRepository : Repository<Category, ApplicationDbContext>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}