﻿using Common;

namespace Basket.API.Models;

public class BasketItem
{
  public string ProductId { get; set; } = string.Empty;
  public string UnitId { get; set; } = string.Empty;
  public string UnitDetail { get; set; } = string.Empty;
  public string ProductName { get; set; } = string.Empty;
  public double Price { get; set; }
  public int Quantity { get; set; }
  public string PictureUri { get; set; } = string.Empty;

  public Error SetQuantity(int quantity)
  {
    if (quantity < 1)
      return Errors.InvalidQuantity;

    Quantity = quantity;
    return Error.None;
  }

  public Error SetPrice(double price)
  {
    if (price < 0)
      return Errors.InvalidPrice;

    Price = price;
    return Error.None;
  }

  public double ToPrice() => Price * Quantity;
}
