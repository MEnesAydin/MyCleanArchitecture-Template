using CleanArchitectureTemplate.Application.Common;
using CleanArchitectureTemplate.Application.Users;
using MediatR;

namespace CleanArchitectureTemplate.WebAPI.Modules;

public static class UserModule
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/users").WithTags("Users").RequireAuthorization();
        group.MapPost("register",
            async (ISender sender, UserCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .AllowAnonymous();
    }
}
