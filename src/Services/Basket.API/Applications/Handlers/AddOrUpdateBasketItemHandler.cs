using API.Contract.Baskets.Requests;
using API.Contract.Baskets.Responses;
using Basket.API.Models;
using Basket.API.Repositories;
using Common;
using Common.Auth;
using Common.Mediator;

namespace Basket.API.Applications.Handlers;

public sealed class AddOrUpdateBasketItemHandler : IRequestHandler<AddOrUpdateBasketItemRequest>
{
  private readonly IUserSessionContext _session;
  private readonly IBasketRepository _repository;
  private readonly ProductRpc.ProductRpcClient _productGrpcClient;

  public AddOrUpdateBasketItemHandler(
    IUserSessionContext session,
    IBasketRepository repository,
    ProductRpc.ProductRpcClient productGrpcClient)
  {
    _session = session;
    _repository = repository;
    _productGrpcClient = productGrpcClient;
  }

  public async Task<Result> Handle(AddOrUpdateBasketItemRequest request)
  {
    var basket = await _repository.GetBasket(_session.UserId)
        ?? new CustomerBasket() { CustomerId = _session.UserId };

    // check product is valid or not
    var unitMessageRequest = new ProductUnitMessageRequest { UnitId = request.UnitId, ProductId = request.ProductId };
    var unit = await _productGrpcClient.GetProductUnitInformationAsync(unitMessageRequest);
    if (unit is null)
      return Result.ValidateFailed(Errors.InvalidProduct);

    // check quantity is valid or not
    if (unit.Stock < request.Quantity)
      return Result.ValidateFailed(Errors.InvalidQuantity);

    var basketItem = new BasketItem
    {
      ProductId = unit.ProductId,
      UnitId = unit.UnitId,
      ProductName = unit.Name,
      PictureUri = unit.Picture,
      UnitDetail = $"Kích cỡ: {unit.Size}, Màu: {unit.Color}",
    };

    var errors = Error.Verify([basketItem.SetQuantity(request.Quantity), basketItem.SetPrice(unit.Price)]);
    if (errors is not null)
      return Result.ValidateFailed([..errors]);

    basket.AddOrUpdate(basketItem);

    await _repository.UpdateBasket(basket);

    var response = new BasketChangedResponse()
    {
      UnitId = request.UnitId,
      NewTotalItemPrice = basketItem.ToPrice(),
      NewQuantity = request.Quantity,
      NewTotalBasketPrice = basket.TotalPrice
    };

    return Result.Ok(response);
  }
}
