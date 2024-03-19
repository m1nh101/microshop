namespace Basket.API.Responses;

public sealed record BasketChangedResponse
{
  public required string ProductId { get; init; }
  public required double TotalItemPrice { get; init; }
}
