using Common.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.API.Infrastructure.Database.Configurations;

public abstract class AuditableEntityConfiguration<TEntity, TKey> : IdentityEntityConfiguration<TEntity, TKey>, IEntityTypeConfiguration<TEntity>
  where TEntity : class, IAuditable, IIdentity<TKey>
{
  public virtual void Configure(EntityTypeBuilder<IAuditable> builder)
  {
    builder.Property(e => e.CreatedBy)
      .HasColumnType("nvarchar(450)")
      .HasMaxLength(450)
      .IsRequired(true);
    builder.Property(e => e.ModifiedBy).HasColumnType("nvarchar(450)")
      .HasMaxLength(450)
      .IsRequired(false);

    builder.Property(e => e.CreatedAt).IsRequired(true);
    builder.Property(e => e.ModifiedAt).IsRequired(false);
  }
}
