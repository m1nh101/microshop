using FastEndpoints;

namespace Product.API.Http.Requests;

public sealed record GetProductByIdRequest
{
    [QueryParam]
    public required string Id { get; init; }
}
