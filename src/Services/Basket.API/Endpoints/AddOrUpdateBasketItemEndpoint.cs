using Auth;
using Basket.API.Repositories;
using Basket.API.Requests;
using Basket.API.Responses;
using Common;
using FastEndpoints;

namespace Basket.API.Endpoints;

public sealed class AddOrUpdateBasketItemEndpoint : Endpoint<AddOrUpdateBasketItemRequest, Result<BasketChangedResponse>>
{
  private readonly IUserSessionContext _session;
  private readonly IBasketRepository _repository;

  public AddOrUpdateBasketItemEndpoint(IUserSessionContext session, IBasketRepository repository)
  {
    _session = session;
    _repository = repository;
  }

  public override async Task HandleAsync(AddOrUpdateBasketItemRequest req, CancellationToken ct)
  {
    var basket = await _repository.GetBasket(_session.UserId);
    if (basket is null)
    {
      await SendAsync(
        response: Errors.BasketNotFound,
        statusCode: 400,
        cancellation: ct);
      return;
    }

    
  }
}
