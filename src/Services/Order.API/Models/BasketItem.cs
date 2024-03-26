namespace Order.API.Models;

public class BasketItem
{
  public string ProductId { get; set; } = string.Empty;
  public string ProductName { get; set; } = string.Empty;
  public double Price { get; set; }
  public int Quantity { get; set; }
  public string PictureUri { get; set; } = string.Empty;
}
