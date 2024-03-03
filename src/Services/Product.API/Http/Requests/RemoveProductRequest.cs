namespace Product.API.Http.Requests;

public record RemoveProductRequest
{
    public required string Id { get; init; }
}