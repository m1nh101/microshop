using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.API.Infrastructure.Entities;

namespace Product.API.Infrastructure.Database.Configurations;

public sealed class ProductItemEntityConfiguration : IEntityTypeConfiguration<ProductItem>
{
  public void Configure(EntityTypeBuilder<ProductItem> builder)
  {
    builder.ToTable("Products");

    builder.HasKey(e => e.Id);

    builder.Property(e => e.Name)
      .HasMaxLength(512)
      .HasColumnType("nvarchar(512)")
      .IsRequired(true);

    builder.HasOne(e => e.Type)
      .WithMany(e => e.Products)
      .HasForeignKey(e => e.TypeId);

    builder.HasOne(e => e.Brand)
      .WithMany(e => e.Products)
      .HasForeignKey(e => e.BrandId);
  }
}
