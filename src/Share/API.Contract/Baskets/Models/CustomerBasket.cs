namespace API.Contract.Baskets.Models;
public class CustomerBasket
{
  public string CustomerId { get; set; } = string.Empty;
  public List<BasketItem> Items { get; set; } = [];
}
