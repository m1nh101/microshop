using Common.Auth;
using Common.Contracts;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Entities;

namespace Product.API.Infrastructure.Database;

public class ProductDbContext : DbContext
{
  private readonly IUserSessionContext _session;

  public ProductDbContext(DbContextOptions<ProductDbContext> options, IUserSessionContext session)
    : base(options)
  {
    _session = session;
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    foreach(var entry in ChangeTracker.Entries<IAuditable>())
    {
      switch(entry.State) {
        case EntityState.Added:
          entry.Entity.CreatedAt = DateTime.UtcNow;
          entry.Entity.CreatedBy = _session.UserId;
          break;
        case EntityState.Modified:
          entry.Entity.ModifiedAt = DateTime.UtcNow;
          entry.Entity.ModifiedBy = _session.UserId;
          break;
        default:
          break;
      }
    }

    return base.SaveChangesAsync(cancellationToken);
  }

  public virtual DbSet<ProductItem> Products => Set<ProductItem>();
  public virtual DbSet<ProductBrand> Brands => Set<ProductBrand>();
  public virtual DbSet<ProductCollection> Collections => Set<ProductCollection>();
  public virtual DbSet<Category> Categories => Set<Category>();
  public virtual DbSet<ProductSize> Sizes => Set<ProductSize>();
  public virtual DbSet<ProductColor> Colors => Set<ProductColor>();
}
