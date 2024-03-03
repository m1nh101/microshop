namespace Product.API.Infrastructure.Entities;

public class ProductBrand
{
  private ProductBrand() { }

  public ProductBrand(string name)
  {
    Id = Guid.NewGuid().ToString();
    Name = name;
  }

  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public virtual ICollection<ProductItem> Products { get; private set; } = [];
}