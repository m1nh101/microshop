using Common;
using Common.Contracts;

namespace Product.API.Infrastructure.Entities;

public class ProductUnit : IIdentity<string>, ICreateable, IAuditable
{
  private ProductUnit() { }

  public ProductUnit(int stock, double price, ProductSize size, ProductColor color)
  {
    Id = Guid.NewGuid().ToString();
    Stock = stock;
    Price = price;
    Size = size;
    Color = color;
  }

  public string Id { get; private set; } = string.Empty;
  public int Stock { get; private set; } = 0;
  public double Price { get; private set; } = 0;
  public string ProductId { get; private set; } = null!;
  public ProductItem Product { get; private set; } = null!;
  public ProductSize Size { get; private set; } = null!;
  public ProductColor Color { get; private set; } = null!;
  public string ColorId { get; private set; } = string.Empty;
  public string SizeId { get; private set; } = string.Empty;

  public DateTime? ModifiedAt { get; set; }

  public string? ModifiedBy { get; set; }

  public DateTime CreatedAt { get; set; }

  public string CreatedBy { get; set; } = string.Empty;

  public void Update(double price, int stock)
  {
    Price = price;
    Stock = stock;
  }

  public void ReduceStock(int quantity)
  {
    if (quantity < 0)
      return;

    Stock -= quantity;
  }

  public void AddStock(int quantity)
  {
    if (quantity < 0)
      return;

    Stock += quantity;
  }
}
