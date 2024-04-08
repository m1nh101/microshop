using Common;
using Common.Auth;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Order.API.Applications.Orders.Responses;
using Order.API.Infrastructure;

namespace Order.API.Applications.Orders.Handlers;

public sealed record GetOrderDetailRequest(
  string OrderId);

public class GetOrderDetailHandler : IRequestHandler<GetOrderDetailRequest>
{
  private readonly OrderDbContext _context;
  private readonly IUserSessionContext _session;

  public GetOrderDetailHandler(
    OrderDbContext context,
    IUserSessionContext session)
  {
    _context = context;
    _session = session;
  }

  public async Task<Result> Handle(GetOrderDetailRequest request)
  {
    var order = await _context.Orders.FirstOrDefaultAsync(e => e.Id == request.OrderId && e.UserId == _session.UserId);
    if (order is null)
      return Result.Failed(Errors.NotFound);

    return Result.Ok(new CustomerOrderResponse(
      order.Id,
      order.GetTotal(),
      order.Status,
      order.CreatedAt,
      order.Items));
  }
}
