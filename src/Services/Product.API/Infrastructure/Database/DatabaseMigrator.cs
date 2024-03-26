using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Entities;

namespace Product.API.Infrastructure.Database;

public class DatabaseMigrator
{
  private readonly IServiceProvider _provider;

  public DatabaseMigrator(IServiceProvider provider)
  {
    _provider = provider;
  }

  public async Task Migrate()
  {
    using var scope = _provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
    {
      await context.Database.MigrateAsync();

      var types = SeedingType();
      var brands = SeedingBrand();

      await SeedProduct(context, types, brands);
    }
  }

  static async Task SeedProduct(ProductDbContext context, ProductType[] types, ProductBrand[] brands)
  {
    var random = new Random();
    var products = new ProductItem[]
    {
      new("Áo phông hè 2020", 10, 100000, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
      new("Áo phông hè 2021", 5, 150000, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
      new("Áo phông hè 2022", 1, 123000, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
      new("Áo phông hè 2023", 23, 234000, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
      new("Quần short hè 2020", 3, 5420000, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
      new("Quần short hè 2021", 12, 120000, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
      new("Điện thoại Android 2021", 50, 120000, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
      new("FruitPhone 2022", 12, 591200, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
      new("FruitPhone 2023", 80, 243200, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
      new("MacPhone 2023", 10, 123400, "https://empty", brands[random.Next(0, brands.Length)], types[random.Next(0, types.Length)]),
    };

    await context.Products.AddRangeAsync(products);
    await context.SaveChangesAsync();
  }

  static ProductBrand[] SeedingBrand()
  {
    return [
      new ProductBrand("Nike"),
      new ProductBrand("Adidas"),
      new ProductBrand("Apple"),
      new ProductBrand("Samsung"),
      new ProductBrand("Amazone"),
    ];
  }

  static ProductType[] SeedingType()
  {
    return
    [
      new("TPCN"),
      new("Drug")
    ];
  }
}