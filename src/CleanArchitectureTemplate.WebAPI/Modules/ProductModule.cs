using MediatR;
using CleanArchitectureTemplate.Application.Common;
using CleanArchitectureTemplate.Application.Products;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.WebAPI.Modules;

public static class ProductModule
{
    public static void RegisterProductRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/products").WithTags("Products").RequireAuthorization();
        group.MapPost(string.Empty,
            async (ISender sender, ProductCreateCommand request, CancellationToken cancellatioNToken) =>
            {
                var response = await sender.Send(request, cancellatioNToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

        group.MapGet(string.Empty,
            async (ISender sender, Guid id, CancellationToken cancellatioNToken) =>
            {
                var response = await sender.Send(new ProductGetQuery(id), cancellatioNToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<Product>>();
    }
}
