using Common;

namespace Order.API;

public static class Errors
{
  public static readonly Error EmptyBasket = new("Order.Create", "Cannot create order without item");
  public static readonly Error NotFound = new("Order", "No order found");
}
