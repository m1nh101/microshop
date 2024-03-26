using Microsoft.EntityFrameworkCore;
using Order.API.Infrastructure.Entities;

namespace Order.API.Infrastructure;

public class OrderDbContext : DbContext
{
  public OrderDbContext(DbContextOptions<OrderDbContext> options)
    : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<BuyerOrder>(cfg =>
    {
      cfg.ToTable("Orders");
      cfg.HasKey(e => e.Id);

      cfg.OwnsMany(e => e.Items)
        .WithOwner()
        .HasForeignKey("OrderId");

      cfg.OwnsOne(e => e.ShippingAddress)
        .WithOwner()
        .HasForeignKey("OrderId");
    });
  }

  public virtual DbSet<BuyerOrder> Orders => Set<BuyerOrder>();
}
