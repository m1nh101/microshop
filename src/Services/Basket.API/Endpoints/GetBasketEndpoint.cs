using Auth;
using Basket.API.Models;
using Basket.API.Repositories;
using Basket.API.Requests;
using Common;
using FastEndpoints;

namespace Basket.API.Endpoints;

[HttpGet("/api/baskets")]
public sealed class GetBasketEndpoint : Endpoint<GetBasketRequest, Result<CustomerBasket>>
{
  private readonly IUserSessionContext _session;
  private readonly IBasketRepository _repository;

  public GetBasketEndpoint(IUserSessionContext session, IBasketRepository repository)
  {
    _session = session;
    _repository = repository;
  }

  public override async Task HandleAsync(GetBasketRequest req, CancellationToken ct)
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

    await SendAsync(
      response: basket,
      statusCode: 200,
      cancellation: ct);
  }
}
