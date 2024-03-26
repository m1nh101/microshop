using Auth;
using Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Order.API.Http.Responses;
using Order.API.Infrastructure;

namespace Order.API.Http.Endpoints;

[HttpGet("/api/orders")]
public sealed class GetOrderByUserEndpoint : Endpoint<EmptyRequest, Result<IEnumerable<CustomerOrderResponse>>>
{
  private readonly OrderDbContext _context;
  private readonly IUserSessionContext _session;

  public GetOrderByUserEndpoint(
    OrderDbContext context,
    IUserSessionContext session)
  {
    _context = context;
    _session = session;
  }

  public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
  {
    var orders = await _context.Orders
      .AsNoTracking()
      .Where(e => e.UserId == _session.UserId)
      .OrderByDescending(e => e.CreatedAt)
      .ToListAsync(ct);

    if(orders is null)
    {
      await SendAsync(
        response: new List<CustomerOrderResponse>(),
        statusCode: 200,
        cancellation: ct);
      return;
    }

    var data = orders.Select(e => new CustomerOrderResponse(
      Id: e.Id,
      TotalPayment: e.Items.Sum(d => d.Price * d.Quantity),
      Status: e.Status,
      CreatedAt: e.CreatedAt,
      Items: e.Items))
      .ToList();

    await SendAsync(
      response: data,
      statusCode: 200,
      cancellation: ct);
  }
}
