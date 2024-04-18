using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.API.Infrastructure.Entities;

namespace Product.API.Infrastructure.Database.Configurations;

public sealed class ProductUnitEntityConfiguration : AuditableEntityConfiguration<ProductUnit, string>
{
  public override void Configure(EntityTypeBuilder<ProductUnit> builder)
  {
    base.Configure(builder);

    builder.ToTable("ProductUnits");

    builder.HasOne(e => e.Product)
      .WithMany(e => e.Units)
      .HasForeignKey(e => e.ProductId);

    builder.HasOne(e => e.Color)
      .WithMany(e => e.ProductUnits)
      .HasForeignKey(e => e.ColorId);

    builder.HasOne(e => e.Size)
      .WithMany(e => e.Units)
      .HasForeignKey(e => e.SizeId);
  }
}