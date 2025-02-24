using CleanArhictecture_2025.Domain.Abstractions;
using CleanArhictecture_2025.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CleanArchitectureTemplate.Domain.Products;
using CleanArchitectureTemplate.Domain.Categories;
using CleanArchitectureTemplate.Application.Categories;

namespace CleanArchitectureTemplate.Application.Products;

public sealed record ProductGetAllQuery() : IRequest<IQueryable<ProductGetAllQueryResponse>>;

public sealed class ProductGetAllQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public CategoryGetAllQueryResponse? Category { get; set; }
}

internal sealed class ProductGetAllQueryHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    UserManager<AppUser> userManager) : IRequestHandler<ProductGetAllQuery, IQueryable<ProductGetAllQueryResponse>>
{
    public Task<IQueryable<ProductGetAllQueryResponse>> Handle(ProductGetAllQuery request, CancellationToken cancellationToken)
    {

        var response = (from entity in productRepository.GetAll()
                        join create_user in userManager.Users.AsQueryable() on entity.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on entity.CreateUserId equals update_user.Id into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        join category in categoryRepository.GetAll() on entity.CategoryId equals category.Id // kategori join
                        select new ProductGetAllQueryResponse
                        {
                            Name = entity.Name,
                            Price = entity.Price,
                            Id = entity.Id,
                            IsActive = entity.IsActive,
                            CreateAt = entity.CreateAt,
                            CreateUserId = entity.CreateUserId,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateAt = entity.UpdateAt,
                            UpdateUserId = entity.UpdateUserId,
                            UpdateUserName = entity.UpdateUserId == null
                                ? null
                                : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                            IsDeleted = entity.IsDeleted,
                            DeleteAt = entity.DeleteAt,
                            Category = new CategoryGetAllQueryResponse
                            {
                                // Kategori bilgilerini doldurun
                                Id = category.Id,
                                Name = category.Name
                            }
                        });
        return Task.FromResult(response);
    }
}



