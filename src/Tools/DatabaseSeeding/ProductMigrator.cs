using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;
using Product.API.Infrastructure.Entities;

namespace DatabaseSeeding;
public class ProductMigrator
{
  private readonly ProductDbContext _context;

  public ProductMigrator(ProductDbContext context)
  {
    _context = context;
  }

  public async Task Seeding()
  {
    if(_context.Products.Any())
    {
      await _context.Products.ExecuteDeleteAsync();
      await _context.Brands.ExecuteDeleteAsync();
      await _context.Categories.ExecuteDeleteAsync();
      await _context.Colors.ExecuteDeleteAsync();
      await _context.Sizes.ExecuteDeleteAsync();
    }

    var brands = SeedingBrand();
    var sizes = SeedingSize();
    var colors = SeedingColor();
    var categories = SeedingCategory();

    await SeedProduct(_context, sizes, colors, categories, brands);
  }

  static async Task SeedProduct(ProductDbContext context, ProductSize[] sizes, ProductColor[] colors, Category[] categories, ProductBrand[] brands)
  {
    var random = new Random();
    var products = new List<ProductItem>();

    for (var i = 0; i < 10000; i++)
    {
      var category = categories[random.Next(0, categories.Length)];
      var price = random.Next(1000, 10000) * random.NextDouble();
      var product = new ProductItem(
        name: $"{category.Name} {random.Next(2000, 2025)}",
        price: price,
        picture: "empty",
        brand: brands[random.Next(brands.Length)],
        material: "Vải").AssignToCategories(category);

      for (var j = 0; j < 10; j++)
      {
        var additionPrice = random.Next(100, 10000) * random.NextDouble();
        var unit = new ProductUnit(
          stock: random.Next(0, 100),
          price: additionPrice,
          size: sizes[random.Next(sizes.Length)],
          color: colors[random.Next(colors.Length)]);

        product.AddUnit(unit);
      }

      product.CreatedAt = DateTime.Now;
      product.CreatedBy = "Migrator";
      products.Add(product);
    }

    await context.Products.AddRangeAsync(products);
    await context.SaveChangesAsync(true);
  }

  static ProductBrand[] SeedingBrand()
  {
    return [
      new ProductBrand("Nike"),
      new ProductBrand("Adidas"),
      new ProductBrand("Apple"),
      new ProductBrand("Samsung"),
      new ProductBrand("Amazone"),
      new ProductBrand("H&M"),
      new ProductBrand("Karetes"),
      new ProductBrand("LV"),
      new ProductBrand("GC"),
    ];
  }

  static ProductSize[] SeedingSize()
  {
    var sizes = new List<ProductSize>();
    string[] sizeText = ["L", "S", "SM", "M", "XL", "XXL"];

    foreach (var size in sizeText)
    {
      sizes.Add(new ProductSize(size, SizeGenre.Kid));
      sizes.Add(new ProductSize(size, SizeGenre.Men));
      sizes.Add(new ProductSize(size, SizeGenre.Women));
    }

    return [.. sizes];
  }

  static ProductColor[] SeedingColor()
  {
    var colors = new HashSet<ProductColor>();
    var random = new Random();

    for (var i = 0; i < 100000; i++)
    {
      var hex = string.Format("#{0:X6}", random.Next(0x1000000));
      var color = new ProductColor(hex, hex);

      if (colors.Contains(color))
        continue;

      colors.Add(color);
    }

    return [.. colors];
  }

  static Category[] SeedingCategory()
  {
    return [
      new Category("Sơ mi"),
      new Category("Quần bò nam"),
      new Category("Quần bò nữ"),
      new Category("Áo bò nam"),
      new Category("Áo bò nữ"),
      new Category("Áo thun nam"),
      new Category("Áo thun nữ"),
      new Category("Quần short nam"),
      new Category("Quần short nữ"),
      new Category("Áo khoác nam"),
      new Category("Áo khoác nữ"),
      new Category("Áo len"),
      new Category("Công sở"),
      new Category("Đi chơi")
    ];
  }
}
