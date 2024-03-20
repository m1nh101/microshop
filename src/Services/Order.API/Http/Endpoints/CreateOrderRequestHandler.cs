using Auth;
using FastEndpoints;
using Order.API.Entities;
using Order.API.Http.Requests;
using Order.API.Infrastructure;
using Order.API.RPC.Clients;

namespace Order.API.Http.Endpoints;

[HttpPost("/api/orders/")]
public sealed class CreateOrderRequestHandler : Endpoint<CreateOrderRequest>
{
  private readonly IUserSessionContext _session;
  private readonly OrderDbContext _context;
  private readonly BasketRpcClient _basketClient;

  public CreateOrderRequestHandler(
    OrderDbContext context,
    IUserSessionContext session,
    BasketRpcClient basketClient)
  {
    _context = context;
    _session = session;
    _basketClient = basketClient;
  }

  public override async Task HandleAsync(CreateOrderRequest req, CancellationToken ct)
  {
    var itemsInBasket = await _basketClient.GetBasket(_session.UserId);
    var items = itemsInBasket.Select(e => new OrderItem(e.ProductId, e.ProductName, e.PictureUri, e.Price, e.Quantity));
    var basket = new BuyerOrder(_session.UserId, items);

    await _context.Orders.AddAsync(basket, ct);
    await _context.SaveChangesAsync(ct);

    await SendAsync(null, 204, ct);
  }
}
