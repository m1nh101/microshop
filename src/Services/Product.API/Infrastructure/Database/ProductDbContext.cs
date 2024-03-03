using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Entities;

namespace Product.API.Infrastructure.Database;

public class ProductDbContext : DbContext
{
  public ProductDbContext(DbContextOptions<ProductDbContext> options)
    : base(options)
  { 
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
  }

  public virtual DbSet<ProductItem> Products => Set<ProductItem>();
  public virtual DbSet<ProductType> Types => Set<ProductType>();
  public virtual DbSet<ProductBrand> Brands => Set<ProductBrand>();
}
