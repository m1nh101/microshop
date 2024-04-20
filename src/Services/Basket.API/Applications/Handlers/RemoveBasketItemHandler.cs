using API.Contract.Baskets.Requests;
using API.Contract.Baskets.Responses;
using Basket.API.Repositories;
using Common;
using Common.Auth;
using Common.Mediator;

namespace Basket.API.Applications.Handlers;

public class RemoveBasketItemHandler : IRequestHandler<RemoveBasketItemRequest>
{
  private readonly IUserSessionContext _session;
  private readonly IBasketRepository _repository;

  public RemoveBasketItemHandler(
    IUserSessionContext session,
    IBasketRepository repository)
  {
    _session = session;
    _repository = repository;
  }

  public async Task<Result> Handle(RemoveBasketItemRequest request)
  {
    var basket = await _repository.GetBasket(_session.UserId);
    if (basket is null)
      return Result.Failed(Errors.BasketNotFound);

    var error = basket.RemoveItem(request.ProductId, request.UnitId);
    if (error.Equals(Error.None))
    {
      await _repository.UpdateBasket(basket);
      var data = new BasketChangedResponse
      {
        UnitId = request.UnitId,
        NewQuantity = 0,
        NewTotalItemPrice = 0,
        NewTotalBasketPrice = basket.TotalPrice
      };

      return Result.Ok(data);
    }

    return Result.Failed(error);
  }
}
