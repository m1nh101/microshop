namespace API.Contract.Products.Requests;

public sealed record GetProductPaginationRequest
{
  public required int PageIndex { get; init; }
  public string? Name { get; init; } = string.Empty;
  public string[]? Brands { get; init; } = null;
  public string? TypeId { get; init; } = null;


}
