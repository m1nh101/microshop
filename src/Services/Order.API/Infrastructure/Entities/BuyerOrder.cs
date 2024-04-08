namespace Order.API.Infrastructure.Entities;

public class BuyerOrder
{
  private BuyerOrder() { }

  public BuyerOrder(string buyerId, string buyerName, ShippingAddress shippingAddress, IEnumerable<OrderItem> items)
  {
    Id = Guid.NewGuid().ToString();
    Items = items.ToList();
    Status = OrderStatus.WaitConfirm;
    UserId = buyerId;
    CreatedAt = DateTime.Now;
    ShippingAddress = shippingAddress;
  }

  public string Id { get; private set; } = string.Empty;
  public string UserId { get; private set; } = string.Empty;
  public string BuyerName { get; private set; } = string.Empty;
  public ShippingAddress ShippingAddress { get; private set; } = null!;
  public DateTime CreatedAt { get; private set; }

  public virtual ICollection<OrderItem> Items { get; private set; } = [];
  public OrderStatus Status { get; private set; }

  public void SetStatus(OrderStatus status)
  {
    Status = status;
  }

  public double GetTotal() => Items.Sum(e => e.Price * e.Quantity);
}
