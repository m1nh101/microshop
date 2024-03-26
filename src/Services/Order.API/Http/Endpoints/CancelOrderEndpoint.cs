using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Order.API.Http.Requests;
using Order.API.Infrastructure;

namespace Order.API.Http.Endpoints;

[HttpDelete("/api/orders/{orderId}")]
public class CancelOrderEndpoint : Endpoint<CancelOrderRequest, EmptyResponse>
{
  private readonly OrderDbContext _context;

  public CancelOrderEndpoint(OrderDbContext context)
  {
    _context = context;
  }

  public override async Task HandleAsync(CancelOrderRequest req, CancellationToken ct)
  {
    var order = await _context.Orders.FirstOrDefaultAsync(e => e.Id == req.OrderId, ct);
    if (order == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    order.SetStatus(Infrastructure.Entities.OrderStatus.Canceled);
    await _context.SaveChangesAsync(ct);

    await SendAsync(
      response: new EmptyResponse(),
      statusCode: 202,
      cancellation: ct);
  }
}
