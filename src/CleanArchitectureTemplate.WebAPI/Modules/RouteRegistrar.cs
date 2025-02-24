using CleanArchitectureTemplate.WebAPI.Modules;

namespace CleanArhictecture_2025.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterProductRoutes();
        app.RegisterCategoryRoutes();
        app.RegisterUserRoutes();
        app.RegisterAuthRoutes();
    }
}
