namespace API.Contract.Baskets.Responses;

public sealed record BasketChangedResponse
{
  public required string UnitId { get; init; }
  public required int NewQuantity { get; init; }
  public required double NewTotalItemPrice { get; init; }
  public required double NewTotalBasketPrice { get; init; }
}
