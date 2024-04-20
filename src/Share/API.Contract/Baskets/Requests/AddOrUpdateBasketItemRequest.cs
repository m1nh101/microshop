namespace API.Contract.Baskets.Requests;

public sealed record AddOrUpdateBasketItemRequest(
  string ProductId,
  string UnitId,
  int Quantity);
