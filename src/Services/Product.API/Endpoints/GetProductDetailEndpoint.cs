using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Endpoints;

[HttpGet("/api/products/{id}")]
[AllowAnonymous]
public class GetProductDetailEndpoint : Endpoint<GetProductByIdRequest, Result<ProductDetailResponse>>
{
  private readonly ProductDbContext _context;

  public GetProductDetailEndpoint(ProductDbContext context)
  {
    _context = context;
  }

  public override async Task HandleAsync(GetProductByIdRequest req, CancellationToken ct)
  {
    var product = await _context.Products
      .AsNoTracking()
      .Where(e => e.Id == req.Id)
      .Select(e => new ProductDetailResponse(
        e.Id,
        e.Name,
        e.Price,
        e.AvailableStock,
        e.PictureUri,
        e.BrandId,
        e.TypeId,
        e.Description))
      .FirstOrDefaultAsync();

    if (product is null)
    {
      await SendAsync(
        response: Errors.ProductNotFound,
        statusCode: 404,
        cancellation: ct);
      return;
    }

    await SendAsync(
        response: product,
        statusCode: 200,
        cancellation: ct);
  }
}
