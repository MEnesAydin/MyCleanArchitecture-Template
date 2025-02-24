using CleanArhictecture_2025.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Infrastructure.Configurations;

public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(x => x.CreateAt)
               .HasColumnType("timestamp no time zone");

        builder.Property(x => x.UpdateAt)
               .HasColumnType("timestamp no time zone");

        builder.Property(x => x.DeleteAt)
                .HasColumnType("timestamp no time zone");
    }
}
