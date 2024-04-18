using Common.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.API.Infrastructure.Database.Configurations;

public abstract class IdentityEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
  where TEntity : class, IIdentity<TKey>
{
  public virtual void Configure(EntityTypeBuilder<TEntity> builder)
  {
    builder.HasKey(e => e.Id);
  }
}
