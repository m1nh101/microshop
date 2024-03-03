using Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Product.API.Http.Requests;
using Product.API.Http.Responses;
using Product.API.Infrastructure.Database;

namespace Product.API.Http.Endpoints;

[HttpGet("/api/products/{id}")]
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
      .Select(e => new ProductDetailResponse(
        e.Id,
        e.Name,
        e.Price,
        e.AvailableStock,
        e.PictureUri,
        e.Description))
      .FirstOrDefaultAsync(e => e.Id == req.Id, ct);

    if(product is null)
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
