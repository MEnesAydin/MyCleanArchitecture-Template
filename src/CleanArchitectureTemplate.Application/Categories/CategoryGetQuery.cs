using MediatR;
using CleanArchitectureTemplate.Application.Common;
using CleanArchitectureTemplate.Domain.Categories;

namespace CleanArchitectureTemplate.Application.Categories;

public sealed record CategoryGetQuery(Guid Id) : IRequest<Result<Category>>;

internal sealed class CategoryGetQueryHandler(
    ICategoryRepository categoryRepository) : IRequestHandler<CategoryGetQuery, Result<Category>>
{
    public async Task<Result<Category>> Handle(CategoryGetQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (category is null)
        {
            return Result<Category>.Failure("Kategori bulunamadı");
        }
        return category;
    }
}
