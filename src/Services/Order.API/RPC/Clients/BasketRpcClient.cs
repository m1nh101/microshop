using API.Contract.Baskets.Models;
using Order.API.Applications.Contracts;

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
