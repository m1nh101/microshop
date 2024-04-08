using Common;
using Common.Auth;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Order.API.Infrastructure;
using Order.API.Infrastructure.Entities;

namespace Order.API.Applications.Orders.Handlers;

public record CancelOrderRequest(
  string OrderId);

public class CancelOrderHandler : IRequestHandler<CancelOrderRequest>
{
  private readonly OrderDbContext _context;
  private readonly IUserSessionContext _session;

  public CancelOrderHandler(
    OrderDbContext context,
    IUserSessionContext session)
  {
    _context = context;
    _session = session;
  }

  public async Task<object> Handle(CancelOrderRequest request)
  {
    var order = await _context.Orders.FirstOrDefaultAsync(e => e.Id == request.OrderId && e.UserId == _session.UserId);
    if (order is null)
      return Result<EmptyResult>.Failed(Errors.NotFound);

    order.SetStatus(OrderStatus.Canceled);
    await _context.SaveChangesAsync();

    return Result<EmptyResult>.Ok(new EmptyResult());
  }
}
