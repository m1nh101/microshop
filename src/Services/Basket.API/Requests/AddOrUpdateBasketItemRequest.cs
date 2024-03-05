namespace Basket.API.Requests;

public sealed record AddOrUpdateBasketItemRequest(
  string ProductId,
  int Quantity);
