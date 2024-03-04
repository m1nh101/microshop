using Common;
using Redis.OM.Modeling;

namespace Basket.API.Models;

[Document(Prefixes = ["CustomerBaskets"])]
public class CustomerBasket
{
  [RedisIdField]
  public string CustomerId { get; set; } = string.Empty;
  public List<BasketItem> Items { get; set; } = [];

  public Error AddOrUpdate(BasketItem item)
  {
    Error error = Error.None;
    var existItem = Items.FirstOrDefault(e => e.ProductId == item.ProductId);
    if (existItem is null)
      Items.Add(item);
    else
      error = existItem.SetQuantity(item.Quantity);

    return error;
  }
}
