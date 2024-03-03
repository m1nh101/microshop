namespace Product.API.Infrastructure.Entities;

public class ProductType
{
  private ProductType() { }

  public ProductType(string name)
  {
    Id = Guid.NewGuid().ToString();
    Name = name;
  }

  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public virtual ICollection<ProductItem> Products { get; private set; } = [];
}
