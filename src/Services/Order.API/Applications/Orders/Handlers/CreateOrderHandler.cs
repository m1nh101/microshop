using Common;
using Common.Auth;
using Common.EventBus;
using Common.Mediator;
using Order.API.Applications.Contracts;
using Order.API.Applications.Events;
using Order.API.Applications.Orders.Responses;
using Order.API.Infrastructure;
using Order.API.Infrastructure.Entities;

namespace Order.API.Applications.Orders.Handlers;

public record CreateOrderRequest(
  ShippingAddress ShippingAddress);

public class CreateOrderHandler : IRequestHandler<CreateOrderRequest>
{
  private readonly IUserSessionContext _session;
  private readonly IEventBus _eventBus;
  private readonly OrderDbContext _context;
  private readonly IBasketClient _basketClient;

  public CreateOrderHandler(
    IUserSessionContext session,
    OrderDbContext context,
    IBasketClient basketClient,
    IEventBus eventBus)
  {
    _session = session;
    _context = context;
    _basketClient = basketClient;
    _eventBus = eventBus;
  }

  public async Task<Result> Handle(CreateOrderRequest request)
  {
    Console.WriteLine("Start creating order");

    var itemsInBasket = await _basketClient.GetBasket(_session.UserId);
    if (itemsInBasket == null || !itemsInBasket.Any())
      return Result.Failed(Summary.MissingRequirement, new Error(nameof(_session.UserId), "Cannot create order if user does not have any item in basket"));

    var items = itemsInBasket.Select(e => new OrderItem(e.ProductId, e.UnitId, e.ProductName,e.UnitDetail , e.PictureUri, e.Price, e.Quantity));
    var order = new BuyerOrder(_session.UserId, _session.Name, request.ShippingAddress, items);

    await _context.Orders.AddAsync(order);
    await _context.SaveChangesAsync();

    await _eventBus.Publish(new OrderCreatedEvent(
      order.Id,
      order.UserId,
      order.Items));

    return Result.Ok(new CustomerOrderResponse(
      order.Id,
      order.GetTotal(),
      order.Status,
      order.CreatedAt,
      order.Items));
  }
}
