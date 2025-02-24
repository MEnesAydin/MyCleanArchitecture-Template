using MediatR;
using CleanArchitectureTemplate.Application.Common;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.Application.Products;

public sealed record ProductGetQuery(Guid Id) : IRequest<Result<Product>>;

internal sealed class ProductGetQueryHandler(
    IProductRepository productRepository) : IRequestHandler<ProductGetQuery, Result<Product>>
{
    public async Task<Result<Product>> Handle(ProductGetQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (product is null)
        {
            return Result<Product>.Failure("Ürün bulunamadı");
        }
        return product;
    }
}