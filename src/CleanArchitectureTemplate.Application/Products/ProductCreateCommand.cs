using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using CleanArchitectureTemplate.Application.Common;
using CleanArchitectureTemplate.Domain.Products;
using CleanArchitectureTemplate.Domain.Categories;

namespace CleanArchitectureTemplate.Application.Products;

public sealed record ProductCreateCommand(
    string Name,
    decimal Price,
    Guid CategoryId) : IRequest<Result<string>>;

public sealed class ProductCreateCommandValidator : AbstractValidator<ProductCreateCommand>
{
    public ProductCreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ürün adı boş olamaz");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Fiyat 0'dan büyük olmalıdır");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Kategori seçimi zorunludur");
    }
}

internal sealed class ProductCreateCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ProductCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var isCategoryExists = await categoryRepository.AnyAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (!isCategoryExists)
        {
            return Result<string>.Failure("Kategori bulunamadı");
        }
        Product product = request.Adapt<Product>();
        productRepository.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Ürün kaydı başarıyla tamamlandı";
    }
}
