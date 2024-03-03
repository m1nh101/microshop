using Common;

namespace Product.API;

internal static class Errors
{
  public static readonly Error ProductNotFound = new("Product", "Product not found");
  public static readonly Error PositiveStock = new("Catalog.Stock", "Available stock cannot be positive");
  public static readonly Error MaxAvailableStockAllow = new("Catalog.Stock", "Stock doesn't enough to order");
}
