using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using CleanArchitectureTemplate.Application.Categories;
using CleanArchitectureTemplate.Application.Products;

namespace CleanArhictecture_2025.WebAPI.Controllers;

[Route("odata")]
[ApiController]
[EnableQuery]
public class AppODataController(
    ISender sender) : ODataController
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<ProductGetAllQueryResponse>("products");
        builder.EntitySet<CategoryGetAllQueryResponse>("categories");
        return builder.GetEdmModel();
    }

    [HttpGet("products")]
    public async Task<IQueryable<ProductGetAllQueryResponse>> GetAllProducts(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new ProductGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("categories")]
    public async Task<IQueryable<CategoryGetAllQueryResponse>> GetAllCategories(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new CategoryGetAllQuery(), cancellationToken);
        return response;
    }


}
