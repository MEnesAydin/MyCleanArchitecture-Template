using MediatR;
using CleanArchitectureTemplate.Application.Categories;
using CleanArchitectureTemplate.Application.Common;
using CleanArchitectureTemplate.Domain.Categories;

namespace CleanArchitectureTemplate.WebAPI.Modules;

public static class CategoryModule
{
    public static void RegisterCategoryRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/categories").WithTags("Categories").RequireAuthorization();
        group.MapPost(string.Empty,
            async (ISender sender, CategoryCreateCommand request, CancellationToken cancellatioNToken) =>
            {
                var response = await sender.Send(request, cancellatioNToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
        group.MapGet(string.Empty,
            async (ISender sender, Guid id, CancellationToken cancellatioNToken) =>
            {
                var response = await sender.Send(new CategoryGetQuery(id), cancellatioNToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<Category>>();
    }
}
