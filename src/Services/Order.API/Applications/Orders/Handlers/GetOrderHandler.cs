using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Order.API.Applications.Orders.Responses;
using Order.API.Infrastructure;

namespace Order.API.Applications.Orders.Handlers;

public sealed record GetOrderRequest;

public class GetOrderHandler : IRequestHandler<GetOrderRequest>
{
  private readonly OrderDbContext _context;

  public GetOrderHandler(OrderDbContext context)
  {
    _context = context;
  }

  public async Task<object> Handle(GetOrderRequest request)
  {
    var orders = await _context.Orders.AsNoTracking()
      .Select(e => new OrderResponse(
        e.Id,
        e.UserId,
        e.BuyerName,
        e.GetTotal(),
        e.ShippingAddress.Address,
        e.Status,
        e.CreatedAt,
        e.Items))
      .ToListAsync();

    return Result<IEnumerable<OrderResponse>>.Ok(orders);
  }
}
