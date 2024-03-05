using Auth;
using Basket.API.Repositories;
using Basket.API.Requests;
using Basket.API.Responses;
using Common;
using FastEndpoints;

namespace Basket.API.Endpoints;

[HttpDelete("/api/baskets/items/{productId}")]
public sealed class RemoveBasketItemEndpoint : Endpoint<RemoveBasketItemRequest, Result<BasketChangedResponse>>
{
  private readonly IUserSessionContext _session;
  private readonly IBasketRepository _repository;

  public RemoveBasketItemEndpoint(IUserSessionContext session, IBasketRepository repository)
  {
    _session = session;
    _repository = repository;
  }

  public override async Task HandleAsync(RemoveBasketItemRequest req, CancellationToken ct)
  {
    var basket = await _repository.GetBasket(_session.UserId);
    if(basket is null)
    {
      await SendAsync(
        response: Errors.BasketNotFound,
        statusCode: 400,
        cancellation: ct);
      return;
    }

    var error = basket.RemoveItem(req.ProductId);
    if(error is not null)
    {
      await SendAsync(
        response: error,
        statusCode: 400,
        cancellation: ct);
      return;
    }

    await _repository.UpdateBasket(basket);
    await SendAsync(
      response: new BasketChangedResponse(),
      statusCode: 200,
      cancellation: ct);
  }
}
