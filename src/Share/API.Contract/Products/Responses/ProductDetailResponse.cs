namespace API.Contract.Products.Responses;

public record ProductDetailResponse
{
  public required string Id { get; init; }
  public required string Name { get; init; }
  public required double Price { get; init; }
  public required string Material { get; init; }
  public required string Picture { get; init; }
  public string Description { get; init; } = string.Empty;
  public required string BrandName { get; init; }
  public required string BrandId { get; init; }
  public IEnumerable<UnitDetail> Units { get; init; } = [];
}

public record UnitDetail(
  string Id,
  string SizeId,
  string Size,
  string ColorId,
  string ColorName,
  double AdditionPrice,
  int Stock);