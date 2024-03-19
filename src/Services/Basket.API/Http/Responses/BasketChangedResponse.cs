namespace Basket.API.Http.Responses;

public sealed record BasketChangedResponse
{
    public required string ProductId { get; init; }
    public required double TotalItemPrice { get; init; }
}
