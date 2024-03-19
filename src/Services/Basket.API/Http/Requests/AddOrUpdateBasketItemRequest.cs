namespace Basket.API.Http.Requests;

public sealed record AddOrUpdateBasketItemRequest(
  string ProductId,
  int Quantity);
