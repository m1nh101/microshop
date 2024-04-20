using Common;
using Redis.OM.Modeling;

namespace Basket.API.Models;

[Document(Prefixes = ["CustomerBaskets"], StorageType = StorageType.Json)]
public class CustomerBasket
{
  [RedisIdField, Indexed]
  public string CustomerId { get; set; } = string.Empty;

  public List<BasketItem> Items { get; set; } = [];
  public double TotalPrice => Items.Sum(e => e.ToPrice());

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
  public Error RemoveItem(string productId, string unitId)
  {
    var item = Items.FirstOrDefault(e => e.ProductId == productId && e.UnitId == unitId);
    if (item is null)
      return Errors.ItemNotExist;

    Items.Remove(item);
    return Error.None;
  }
}
