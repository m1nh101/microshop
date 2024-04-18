using Common.Contracts;

namespace Product.API.Infrastructure.Entities;

public class ProductColor : IIdentity<string>
{
  private ProductColor() { }

  public ProductColor(string name, string hex)
  {
    Id = Guid.NewGuid().ToString();
    Name = name;
    Hex = hex;
  }

  public string Id { get; private set; } = string.Empty;
  public string Name { get; private set; } = string.Empty;
  public string Hex { get; private set; } = string.Empty;

  public virtual ICollection<ProductUnit> ProductUnits { get; set; } = null!;
}