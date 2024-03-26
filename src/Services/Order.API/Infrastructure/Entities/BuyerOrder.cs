namespace Order.API.Infrastructure.Entities;

public class BuyerOrder
{
  private BuyerOrder() { }

  public BuyerOrder(string userId, ShippingAddress shippingAddress, IEnumerable<OrderItem> items)
  {
    Id = Guid.NewGuid().ToString();
    Items = items.ToList();
    Status = OrderStatus.WaitConfirm;
    UserId = userId;
    CreatedAt = DateTime.Now;
    ShippingAddress = shippingAddress;
  }

  public string Id { get; private set; } = string.Empty;
  public string UserId { get; private set; } = string.Empty;
  public ShippingAddress ShippingAddress { get; private set; } = null!;
  public DateTime CreatedAt { get; private set; }

  public virtual ICollection<OrderItem> Items { get; private set; } = [];
  public OrderStatus Status { get; private set; }

  public void SetStatus(OrderStatus status)
  {
    Status = status;
  }
}
