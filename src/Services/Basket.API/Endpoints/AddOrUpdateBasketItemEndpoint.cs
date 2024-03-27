using API.Contract.Baskets.Requests;
using API.Contract.Baskets.Responses;
using Auth;
using Basket.API.Models;
using Basket.API.Repositories;
using Basket.API.RPC.Clients;
using Common;
using FastEndpoints;

namespace Basket.API.Http.Endpoints;

[HttpPost("/api/baskets/update")]
public sealed class AddOrUpdateBasketItemEndpoint : Endpoint<AddOrUpdateBasketItemRequest, Result<BasketChangedResponse>>
{
  private readonly IUserSessionContext _session;
  private readonly IBasketRepository _repository;
  private readonly ProductRpcClient _productClient;

  public AddOrUpdateBasketItemEndpoint(
    IUserSessionContext session,
    IBasketRepository repository,
    ProductRpcClient productClient)
  {
    _session = session;
    _repository = repository;
    _productClient = productClient;
  }

  public override async Task HandleAsync(AddOrUpdateBasketItemRequest req, CancellationToken ct)
  {
    var basket = await _repository.GetBasket(_session.UserId)
        ?? new CustomerBasket() { CustomerId = _session.UserId };

    // check product is valid or not
    var product = await _productClient.GetProduct(req.ProductId);
    if (product is null)
    {
      await SendAsync(
        response: Errors.InvalidProduct,
        statusCode: 400,
        cancellation: ct);
      return;
    }

    // check quantity is valid or not
    if (product.AvailableStock < req.Quantity)
    {
      await SendAsync(
        response: Errors.InvalidQuantity,
        statusCode: 400,
        cancellation: ct);
      return;
    }

    var basketItem = new BasketItem
    {
      ProductId = product.ProductId,
      ProductName = product.Name,
      PictureUri = product.PictureUri,
    };

    Error[] errors = [basketItem.SetQuantity(req.Quantity), basketItem.SetPrice(product.Price)];
    if (errors.Where(e => !e.Equals(Error.None)).Any())
    {
      await SendAsync(
        response: Result<BasketChangedResponse>.Failed(errors),
        statusCode: 400,
        cancellation: ct);
      return;
    }

    basket.AddOrUpdate(basketItem);

    await _repository.UpdateBasket(basket);

    var response = new BasketChangedResponse()
    {
      ProductId = req.ProductId,
      NewTotalItemPrice = basketItem.ToPrice(),
      NewQuantity = req.Quantity,
      NewTotalBasketPrice = basket.TotalPrice
    };

    await SendAsync(
      response: response,
      statusCode: 200,
      cancellation: ct);
  }
}
