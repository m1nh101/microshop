using Common;

namespace Basket.API;

internal class Errors
{
  public static readonly Error InvalidQuantity = new("Item.Quantity", "Quantity must be least 1");
  public static readonly Error InvalidPrice = new("Item.Price", "Price cannot be negative number");
  public static readonly Error BasketNotFound = new("Basket", "Basket of customer not exist");
}
