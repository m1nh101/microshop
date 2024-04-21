using Common;
using Common.Auth;
using Common.EventBus;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Order.API.Applications.Events;
using Order.API.Infrastructure;
using Order.API.Infrastructure.Entities;

namespace Order.API.Applications.Orders.Handlers;

public record CancelOrderRequest(
  string OrderId);

public class CancelOrderHandler : IRequestHandler<CancelOrderRequest>
{
  private readonly OrderDbContext _context;
  private readonly IUserSessionContext _session;
  private readonly IEventBus _eventBus;
  private const int DaysAllowToCancel = 1;

  public CancelOrderHandler(
    OrderDbContext context,
    IUserSessionContext session,
    IEventBus eventBus)
  {
    _context = context;
    _session = session;
    _eventBus = eventBus;
  }

  public async Task<Result> Handle(CancelOrderRequest request)
  {
    var order = await _context.Orders
      .Include(e => e.Items)
      .FirstOrDefaultAsync(e => e.Id == request.OrderId && e.UserId == _session.UserId);
    if (order is null)
      return Result.Failed(Summary.NotFound, Error.NotFound<BuyerOrder>(_session.UserId));

    if (DateTime.Now.Subtract(order.CreatedAt).Days > DaysAllowToCancel)
      return Result.Failed("Time to cancel order has expired", new Error(nameof(order.CreatedAt), $"Duration allow to cancel order is 1 day, your order has been created since {order.CreatedAt}"));

    order.SetStatus(OrderStatus.Canceled);
    await _context.SaveChangesAsync();

    await _eventBus.Publish(new OrderCanceledEvent(
      order.Id,
      order.Items));

    return Result.Ok();
  }
}
