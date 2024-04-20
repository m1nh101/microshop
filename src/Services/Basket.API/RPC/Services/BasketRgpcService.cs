using Basket.API.Repositories;
using Grpc.Core;

namespace Basket.API.RPC.Services;

public class BasketRgpcService : BasketRgpc.BasketRgpcBase
{
  private readonly IBasketRepository _repository;

  public BasketRgpcService(IBasketRepository repository)
  {
    _repository = repository;
  }

  public override async Task<BasketMessageReply> GetBasket(BasketByUserIdMessageRequest request, ServerCallContext context)
  {
    var response = new BasketMessageReply();
    var basket = await _repository.GetBasket(request.UserId);

    if (basket is null)
      return response;

    var items = basket.Items.Select(e => new BasketItemPartMessage
    {
      UnitId = e.UnitId,
      PictureUrl = e.PictureUri,
      ProductId = e.ProductId,
      ProductName = e.ProductName,
      Price = e.Price,
      Quantity = e.Quantity
    });

    response.Items.AddRange(items);

    return response;
  }
}
