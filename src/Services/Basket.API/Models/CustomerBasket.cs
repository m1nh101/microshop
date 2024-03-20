using Common;
using Redis.OM.Modeling;

namespace Basket.API.Models;

[Document(Prefixes = ["CustomerBaskets"], StorageType = StorageType.Json)]
public class CustomerBasket
{
  [RedisIdField, Indexed]
  public string CustomerId { get; set; } = string.Empty;
  
  public List<BasketItem> Items { get; set; } = [];

  public void AddOrUpdate(BasketItem item)
  {
    var existItem = Items.FirstOrDefault(e => e.ProductId == item.ProductId);
    if (existItem is null)
      Items.Add(item);
    else
    {
      existItem.SetQuantity(item.Quantity);
      existItem.SetPrice(item.Price);
    }
  }

  public Error RemoveItem(string productId)
  {
    var item = Items.FirstOrDefault(e => e.ProductId == productId);
    if (item is null)
      return Errors.ItemNotExist;

    Items.Remove(item);
    return Error.None;
  }

  public double GetTotalPrice() => Items.Sum(e => e.ToPrice());
}
