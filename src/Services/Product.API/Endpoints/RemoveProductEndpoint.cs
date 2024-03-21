using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Endpoints;

[HttpDelete("/api/products/{id}")]
public sealed class RemoveProductEndpoint : Endpoint<RemoveProductRequest, Result<RemoveProductResponse>>
{
    private readonly ProductDbContext _context;

    public RemoveProductEndpoint(ProductDbContext context)
    {
        _context = context;
    }

    public override async Task HandleAsync(RemoveProductRequest req, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == req.Id, ct);

        if (product == null)
        {
            await SendAsync(
              response: Errors.ProductNotFound,
              statusCode: 400,
              cancellation: ct);
            return;
        }

        product.UpdateAvailableStatus(false);

        await _context.SaveChangesAsync(ct);

        await SendAsync(
          response: new RemoveProductResponse(),
          statusCode: 200,
          cancellation: ct);
    }
}
