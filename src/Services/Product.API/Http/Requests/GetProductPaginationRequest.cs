using FastEndpoints;

namespace Product.API.Http.Requests;

public sealed record GetProductPaginationRequest
{
    [QueryParam]
    public required int PageIndex { get; init; }
    [QueryParam]
    public string? Name { get; init; } = string.Empty;
    [QueryParam]
    public string[]? Brands { get; init; } = null;
    [QueryParam]
    public string? TypeId { get; init; } = null;
}
