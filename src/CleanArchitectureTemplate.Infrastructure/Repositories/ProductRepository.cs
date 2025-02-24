using CleanArhictecture_2025.Infrastructure.Context;
using GenericRepository;
using CleanArchitectureTemplate.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Infrastructure.Repositories;

internal sealed class ProductRepository : Repository<Product, ApplicationDbContext>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }
}
