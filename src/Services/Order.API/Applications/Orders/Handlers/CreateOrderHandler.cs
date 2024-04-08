using Common;
using Common.Auth;
using Common.Mediator;
using Order.API.Applications.Orders.Responses;
using Order.API.Infrastructure;
using Order.API.Infrastructure.Entities;
using Order.API.RPC.Clients;

namespace Order.API.Applications.Orders.Handlers;

public record CreateOrderRequest(
  ShippingAddress ShippingAddress);

public class CreateOrderHandler : IRequestHandler<CreateOrderRequest>
{
  private readonly IUserSessionContext _session;
  private readonly OrderDbContext _context;
  private readonly BasketRpcClient _basketClient;

  public CreateOrderHandler(
    IUserSessionContext session,
    OrderDbContext context,
    BasketRpcClient basketClient)
  {
    _session = session;
    _context = context;
    _basketClient = basketClient;
  }

  public async Task<object> Handle(CreateOrderRequest request)
  {
    var itemsInBasket = await _basketClient.GetBasket(_session.UserId);
    if (itemsInBasket == null || !itemsInBasket.Any())
      return Result<CustomerOrderResponse>.Failed(Errors.EmptyBasket);

    var items = itemsInBasket.Select(e => new OrderItem(e.ProductId, e.ProductName, e.PictureUri, e.Price, e.Quantity));
    var order = new BuyerOrder(_session.UserId, _session.Name, request.ShippingAddress, items);

    await _context.Orders.AddAsync(order);
    await _context.SaveChangesAsync();

    return Result<CustomerOrderResponse>.Ok(new CustomerOrderResponse(
      order.Id,
      order.GetTotal(),
      order.Status,
      order.CreatedAt,
      order.Items));
  }
}
