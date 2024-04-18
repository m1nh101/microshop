using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.API.Infrastructure.Entities;

namespace Product.API.Infrastructure.Database.Configurations;

public sealed class ProductItemEntityConfiguration : AuditableEntityConfiguration<ProductItem, string>
{
  public override void Configure(EntityTypeBuilder<ProductItem> builder)
  {
    base.Configure(builder);

    builder.ToTable("Products");

    builder.Property(e => e.Name)
      .HasMaxLength(512)
      .HasColumnType("nvarchar(512)")
      .IsRequired(true);

    builder.Property(e => e.Material)
      .HasColumnType("nvarchar(512)")
      .HasMaxLength(512);

    builder.Property(e => e.PictureUri)
      .HasColumnType("varchar(512)")
      .HasMaxLength(512);

    builder.HasIndex(e => e.Name);

    builder.HasOne(e => e.Brand)
      .WithMany(e => e.Products)
      .HasForeignKey(e => e.BrandId);

    builder.HasOne(e => e.Collection)
      .WithMany(e => e.Products)
      .HasForeignKey(e => e.CollectionId)
      .IsRequired(false);

    builder.HasMany(e => e.Categories)
      .WithMany(e => e.Products)
      .UsingEntity<ProductCategory>(
        l => l.HasOne(d => d.Category)
          .WithMany(d => d.ProductCategories)
          .HasForeignKey(d => d.CategoryId),
        r => r.HasOne(d => d.Product)
          .WithMany(d => d.ProductCategories)
          .HasForeignKey(d => d.ProductId),
        tb =>
        {
          tb.ToTable("ProductCategories");
          tb.HasKey(d => new { d.ProductId, d.CategoryId });
        });
  }
}