using Common;
using Common.Auth;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Order.API.Applications.Orders.Responses;
using Order.API.Infrastructure;

namespace Order.API.Applications.Orders.Handlers;

public sealed record GetUserOrderRequest;

public class GetUserOrderHandler : IRequestHandler<GetUserOrderRequest>
{
  private readonly OrderDbContext _context;
  private readonly IUserSessionContext _session;

  public GetUserOrderHandler(
    OrderDbContext context,
    IUserSessionContext session)
  {
    _context = context;
    _session = session;
  }

  public async Task<object> Handle(GetUserOrderRequest request)
  {
    var orders = await _context.Orders.AsNoTracking()
      .Where(e => e.UserId == _session.UserId)
      .Select(e => new UserOrderResponse(
        e.Id,
        e.GetTotal(),
        e.ShippingAddress.Address,
        e.Status,
        e.CreatedAt,
        e.Items))
      .ToListAsync();

    return Result<IEnumerable<UserOrderResponse>>.Ok(orders);
  }
}
