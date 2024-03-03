using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.API.Infrastructure.Entities;

namespace Product.API.Infrastructure.Database.Configurations;

public sealed class ProductTypeEntityConfiguration : IEntityTypeConfiguration<ProductType>
{
  public void Configure(EntityTypeBuilder<ProductType> builder)
  {
    builder.ToTable("ProductTypes");

    builder.HasKey(e => e.Id);

    builder.Property(e => e.Name)
      .HasMaxLength(512)
      .HasColumnType("nvarchar(512)")
      .IsRequired(true);
  }
}



