using CleanArhictecture_2025.Domain.Abstractions;
using CleanArhictecture_2025.Domain.Users;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Domain.Categories;
using CleanArchitectureTemplate.Domain.Products;
using System.Security.Claims;

namespace CleanArhictecture_2025.Infrastructure.Context;
internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        Guid currentUserId = Guid.Empty;

        if (httpContextAccessor.HttpContext != null &&
            httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated == true)
        {
            string? userIdString = httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrWhiteSpace(userIdString) && Guid.TryParse(userIdString, out Guid userGuid))
            {
                currentUserId = userGuid;
            }
        }

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt).CurrentValue = DateTime.UtcNow;
                entry.Property(p => p.CreateUserId).CurrentValue = currentUserId;
            }
            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt).CurrentValue = DateTime.UtcNow;
                    entry.Property(p => p.DeleteUserId).CurrentValue = currentUserId;
                }
                else
                {
                    entry.Property(p => p.UpdateAt).CurrentValue = DateTime.UtcNow;
                    entry.Property(p => p.UpdateUserId).CurrentValue = currentUserId;
                }
            }
            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız");
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}