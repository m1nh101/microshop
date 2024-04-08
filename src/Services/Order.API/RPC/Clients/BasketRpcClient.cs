using Order.API.Applications.Contracts;
using Order.API.Models;

namespace Order.API.RPC.Clients;

public class BasketRpcClient : IBasketClient
{
  private readonly BasketRgpc.BasketRgpcClient _client;

  public BasketRpcClient(BasketRgpc.BasketRgpcClient client)
  {
    _client = client;
  }

  public async Task<IEnumerable<BasketItem>> GetBasket(string userId)
  {
    var basket = await _client.GetBasketAsync(new GetBasketByUserIdRequest { UserId = userId });

    return basket.Items.Select(e => new BasketItem
    {
      Price = e.Price,
      ProductId = e.ProductId,
      Quantity = e.Quantity,
      ProductName = e.ProductName,
      PictureUri = e.PictureUrl
    });
  }
}
