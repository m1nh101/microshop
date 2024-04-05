using Auth;
using Basket.API.Models;
using Basket.API.Repositories;
using Common;
using Common.Mediator;

namespace Basket.API.Handlers;

public sealed record GetBasketRequest;

public sealed class GetBasketHandler : IRequestHandler<GetBasketRequest>
{
  private readonly IUserSessionContext _session;
  private readonly IBasketRepository _repository;

  public GetBasketHandler(
    IUserSessionContext session,
    IBasketRepository repository)
  {
    _session = session;
    _repository = repository;
  }

  public async Task<object> Handle(GetBasketRequest request)
  {
    var basket = await _repository.GetBasket(_session.UserId);
    if (basket is null)
      return Errors.BasketNotFound;

    return Result<CustomerBasket>.Ok(basket);
  }
}

