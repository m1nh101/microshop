using FastEndpoints;

namespace API.Contract.Products.Requests;

public sealed record GetProductByIdRequest
{
  [QueryParam]
  public required string Id { get; init; }
}
