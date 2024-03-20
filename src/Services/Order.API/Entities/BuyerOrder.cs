namespace Order.API.Entities;

public class BuyerOrder
{
  private BuyerOrder() { }

  public BuyerOrder(string userId, IEnumerable<OrderItem> items)
  {
    Id = Guid.NewGuid().ToString();
    Items = items.ToList();
    Status = OrderStatus.Started;
    UserId = userId;
    CreatedAt = DateTime.Now;
  }

  public string Id { get; private set; } = string.Empty;
  public string UserId { get; private set; } = string.Empty;
  public DateTime CreatedAt { get; private set; }

  public virtual ICollection<OrderItem> Items { get; private set; } = [];
  public OrderStatus Status { get; private set; }

  public void SetStatus(OrderStatus status)
  {
    Status = status;
  }
}

public class OrderItem
{
  private OrderItem() { }
  public OrderItem(string productId, string productName, string pictureUrl, double price, int quantity)
  {
    ProductId = productId;
    ProductName = productName;
    PictureUrl = pictureUrl;
    Price = price;
    Quantity = quantity;
  }

  public string ProductId { get; private set; } = string.Empty;
  public string ProductName { get; private set; } = string.Empty;
  public string PictureUrl { get; private set; } = string.Empty;
  public double Price { get; private set; }
  public int Quantity { get; private set; }
}

public enum OrderStatus
{
  Started = 0,
  Shipping = 1,
  Completed = 2
}