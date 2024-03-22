namespace API.Contract.Products.Requests;

public sealed record GetProductByIdRequest
{
  public required string Id { get; init; }
}
