using Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Product.API.Http.Requests;
using Product.API.Http.Responses;
using Product.API.Infrastructure.Database;

namespace Product.API.Http.Endpoints;

[HttpGet("/api/products")]
public class GetProductPaginationEndpoint : Endpoint<GetProductPaginationRequest, Result<IEnumerable<ProductPaginationResponse>>>
{
  private readonly ProductDbContext _context;

  public GetProductPaginationEndpoint(ProductDbContext context)
  {
    _context = context;
  }

  private const int NumberOfItemPerPage = 25;

  public override async Task HandleAsync(GetProductPaginationRequest req, CancellationToken ct)
  {
    var query = _context.Products.AsNoTracking();

    if (!string.IsNullOrEmpty(req.Name))
      query = query.Where(e => e.Name.StartsWith(req.Name));
    if (req.Brands != null && req.Brands.Length > 0)
      query = query.Where(e => req.Brands.Any(d => d == e.BrandId));
    if (!string.IsNullOrEmpty(req.TypeId))
      query = query.Where(e => e.TypeId == req.TypeId);

    var products = await query
      .Skip(req.PageIndex * NumberOfItemPerPage)
      .Take(NumberOfItemPerPage)
      .Select(e => new ProductPaginationResponse(
        e.Id,
        e.Name,
        e.Price,
        e.PictureUri,
        e.Description))
      .ToListAsync(ct);

    await SendAsync(
      response: products,
      statusCode: 200,
      cancellation: ct);
  }
}
