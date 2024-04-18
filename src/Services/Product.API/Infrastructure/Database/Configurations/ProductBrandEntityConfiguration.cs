using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.API.Infrastructure.Entities;

namespace Product.API.Infrastructure.Database.Configurations;

public sealed class ProductBrandEntityConfiguration : IdentityEntityConfiguration<ProductBrand, string>
{
  public override void Configure(EntityTypeBuilder<ProductBrand> builder)
  {
    builder.ToTable("ProductBrands");

    builder.Property(e => e.Name)
      .HasMaxLength(512)
      .HasColumnType("nvarchar(512)")
      .IsRequired(true);
  }
}