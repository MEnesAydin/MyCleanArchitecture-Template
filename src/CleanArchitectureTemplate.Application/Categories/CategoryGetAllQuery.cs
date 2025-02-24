using CleanArhictecture_2025.Domain.Abstractions;
using CleanArhictecture_2025.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CleanArchitectureTemplate.Domain.Categories;

namespace CleanArchitectureTemplate.Application.Categories;

public sealed record CategoryGetAllQuery() : IRequest<IQueryable<CategoryGetAllQueryResponse>>;

public sealed class CategoryGetAllQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
}

internal sealed class CategoryGetAllQueryHandler(
    ICategoryRepository categoryRepository,
    UserManager<AppUser> userManager) : IRequestHandler<CategoryGetAllQuery, IQueryable<CategoryGetAllQueryResponse>>
{
    public Task<IQueryable<CategoryGetAllQueryResponse>> Handle(CategoryGetAllQuery request, CancellationToken cancellationToken)
    {

        var response = (from entity in categoryRepository.GetAll()
                        join create_user in userManager.Users.AsQueryable() on entity.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on entity.CreateUserId equals update_user.Id into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new CategoryGetAllQueryResponse
                        {
                            Name = entity.Name,

                            Id = entity.Id,
                            IsActive = entity.IsActive,
                            CreateAt = entity.CreateAt,
                            CreateUserId = entity.CreateUserId,
                            CreateUserName =
                                    create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateAt = entity.UpdateAt,
                            UpdateUserId = entity.UpdateUserId,
                            UpdateUserName =
                                    entity.UpdateUserId == null
                                    ? null
                                    : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                            IsDeleted = entity.IsDeleted,
                            DeleteAt = entity.DeleteAt
                        });
        return Task.FromResult(response);
    }
}