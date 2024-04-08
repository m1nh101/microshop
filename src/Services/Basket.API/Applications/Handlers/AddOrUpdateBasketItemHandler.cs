using API.Contract.Baskets.Requests;
using API.Contract.Baskets.Responses;
using Basket.API.Models;
using Basket.API.Repositories;
using Basket.API.RPC.Clients;
using Common;
using Common.Auth;
using Common.Mediator;

namespace Basket.API.Applications.Handlers;

public sealed class AddOrUpdateBasketItemHandler : IRequestHandler<AddOrUpdateBasketItemRequest>
{
  private readonly IUserSessionContext _session;
  private readonly IBasketRepository _repository;
  private readonly ProductRpcClient _productClient;

  public AddOrUpdateBasketItemHandler(
    IUserSessionContext session,
    IBasketRepository repository,
    ProductRpcClient productClient)
  {
    _session = session;
    _repository = repository;
    _productClient = productClient;
  }

  public async Task<Result> Handle(AddOrUpdateBasketItemRequest request)
  {
    var basket = await _repository.GetBasket(_session.UserId)
        ?? new CustomerBasket() { CustomerId = _session.UserId };

    // check product is valid or not
    var product = await _productClient.GetProduct(request.ProductId);
    if (product is null)
      return Result.Failed(Errors.InvalidProduct);

    // check quantity is valid or not
    if (product.AvailableStock < request.Quantity)
      return Result.Failed(Errors.InvalidQuantity);

    var basketItem = new BasketItem
    {
      ProductId = product.ProductId,
      ProductName = product.Name,
      PictureUri = product.PictureUri,
    };

    Error[] errors = [basketItem.SetQuantity(request.Quantity), basketItem.SetPrice(product.Price)];
    if (errors.Where(e => !e.Equals(Error.None)).Any())
      return Result.Failed(errors);

    basket.AddOrUpdate(basketItem);

    await _repository.UpdateBasket(basket);

    var response = new BasketChangedResponse()
    {
      ProductId = request.ProductId,
      NewTotalItemPrice = basketItem.ToPrice(),
      NewQuantity = request.Quantity,
      NewTotalBasketPrice = basket.TotalPrice
    };

    return Result.Ok(response);
  }
}
