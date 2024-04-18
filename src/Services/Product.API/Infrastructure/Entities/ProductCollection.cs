using Common.Contracts;

namespace Product.API.Infrastructure.Entities;

public class ProductCollection : IIdentity<string>
{
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public ICollection<ProductItem> Products { get; set; } = null!;
}