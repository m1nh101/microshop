using Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Order.API.Http.Requests;
using Order.API.Http.Responses;
using Order.API.Infrastructure;

namespace Order.API.Http.Endpoints;

[HttpGet("/api/orders/admin")]
[Authorize(Roles = "admin")]
public class GetOrderEndpoint : Endpoint<GetOrderRequest, Result<IEnumerable<CustomerOrderResponse>>>
{
  private readonly OrderDbContext _context;

  public GetOrderEndpoint(OrderDbContext context)
  {
    _context = context;
  }

  public override Task HandleAsync(GetOrderRequest req, CancellationToken ct)
  {
    var query = _context.Orders.AsNoTracking();

    if (!string.IsNullOrEmpty(req.UserId))
      query = query.Where(e => e.UserId == req.UserId);
    if (req.Status.HasValue)
      query = query.Where(e => e.Status == req.Status);
    if(req.Time.HasValue)
      query = query.Where(e => e.CreatedAt == req.Time);

    return base.HandleAsync(req, ct);
  }
}
