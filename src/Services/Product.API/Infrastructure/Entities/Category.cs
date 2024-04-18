using Common.Contracts;

namespace Product.API.Infrastructure.Entities;

public class Category : IIdentity<string>
{
  private Category()
  {
  }

  public Category(string name)
  {
    Id = Guid.NewGuid().ToString();
    Name = name;
  }

  public string Id { get; private set; } = string.Empty;
  public string Name { get; private set; } = string.Empty;

  public virtual ICollection<ProductCategory> ProductCategories { get; private set; } = null!;
  public virtual ICollection<ProductItem> Products { get; private set; } = null!;
}
