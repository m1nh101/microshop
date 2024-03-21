namespace API.Contract.Products.Requests;

public record RemoveProductRequest
{
  public required string Id { get; init; }
}