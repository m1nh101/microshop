using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Endpoints;

[HttpGet("/api/products")]
[AllowAnonymous]
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
    var query = _context.Products
      .Include(e => e.Type)
      .Include(e => e.Brand)
      .AsNoTracking();
    var brands = req.Brands?.Where(e => !string.IsNullOrEmpty(e)).ToList();

    if (!string.IsNullOrEmpty(req.Name))
      query = query.Where(e => e.Name.StartsWith(req.Name));
    if (brands != null && brands.Count > 0)
      query = query.Where(e => brands.Any(d => d == e.BrandId));
    if (!string.IsNullOrEmpty(req.TypeId))
      query = query.Where(e => e.TypeId == req.TypeId);

    var products = await query
      .Skip(req.PageIndex * NumberOfItemPerPage)
      .Take(NumberOfItemPerPage)
      .Select(e => new ProductPaginationResponse(
        e.Id,
        e.Name,
        e.Price,
        e.Brand.Name,
        e.Type.Name,
        e.PictureUri,
        e.Description))
      .ToListAsync(ct);

    await SendAsync(
      response: products,
      statusCode: 200,
      cancellation: ct);
  }
}
