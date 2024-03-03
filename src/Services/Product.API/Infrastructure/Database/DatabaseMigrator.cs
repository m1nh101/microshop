using Common.IO;
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
    if(pendingMigrations.Any())
    {
      await context.Database.MigrateAsync();

      var types = SeedingType();
      var brands = SeedingBrand();

      await SeedProduct(context, types, brands);
    }
  }

  static async Task SeedProduct(ProductDbContext context, ProductType[] types, ProductBrand[] brands)
  {
    FileReader reader = new CsvFileReader();
    var random = new Random();
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Sources", "product.csv");
    var source = reader.Read<ProductCsvRecord>(filePath);
    var products = source.Select(e => new ProductItem(
      e.Name,
      e.AvailableStock,
      e.Price,
      e.PictureUri,
      brands[random.Next(0, brands.Length)],
      types[random.Next(0, types.Length)],
      e.Description));

    await context.Products.AddRangeAsync(products);
    await context.SaveChangesAsync();
  }

  static ProductBrand[] SeedingBrand()
  {
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Sources", "brand.csv");
    var brands = File.ReadAllLines(filePath).Select(e => new ProductBrand(e));

    return brands.ToArray();
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