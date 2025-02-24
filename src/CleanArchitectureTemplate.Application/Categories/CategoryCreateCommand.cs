using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using CleanArchitectureTemplate.Application.Common;
using CleanArchitectureTemplate.Domain.Categories;

namespace CleanArchitectureTemplate.Application.Categories;

public sealed record CategoryCreateCommand(
    string Name) : IRequest<Result<string>>;

public sealed class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
{
    public CategoryCreateCommandValidator()
    {

    }
}

internal sealed class CategoryCreateCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CategoryCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        Category category = request.Adapt<Category>();
        categoryRepository.Add(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Kategori kaydı başarıyla tamamlandı";
    }
}
