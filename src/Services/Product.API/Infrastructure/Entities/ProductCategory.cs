namespace Product.API.Infrastructure.Entities;

public class ProductCategory
{
  public string CategoryId { get; set; } = string.Empty;
  public string ProductId { get; set; } = string.Empty;

  public virtual ProductItem Product { get; set; } = null!;
  public virtual Category Category { get; set; } = null!;
}
