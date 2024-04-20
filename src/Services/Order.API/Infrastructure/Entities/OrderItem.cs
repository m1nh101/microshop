namespace Order.API.Infrastructure.Entities;

public class OrderItem
{
  private OrderItem() { }
  public OrderItem(string productId, string unitId, string productName, string unitDetail, string pictureUrl, double price, int quantity)
  {
    ProductId = productId;
    ProductName = productName;
    PictureUrl = pictureUrl;
    Price = price;
    Quantity = quantity;
    UnitId = unitId;
    UnitDetail = unitDetail;
  }

  public string ProductId { get; private set; } = string.Empty;
  public string UnitId { get; private set; } = string.Empty;
  public string UnitDetail { get; private set; } = string.Empty;
  public string ProductName { get; private set; } = string.Empty;
  public string PictureUrl { get; private set; } = string.Empty;
  public double Price { get; private set; }
  public int Quantity { get; private set; }
}
