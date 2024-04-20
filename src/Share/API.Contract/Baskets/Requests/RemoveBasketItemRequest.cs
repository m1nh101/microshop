namespace API.Contract.Baskets.Requests;

public sealed record RemoveBasketItemRequest(
  string ProductId,
  string UnitId);
