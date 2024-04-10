using Common.EventBus;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Applications.Events;

public sealed record OrderCanceledEvent(
  string OrderId,
  IEnumerable<OrderItem> Items) : IntergratedEvent;

public sealed class OrderCancelEventHandler : IEventHandler<OrderCanceledEvent>
{
  private readonly ProductDbContext _context;

  public OrderCancelEventHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task Handle(OrderCanceledEvent @event)
  {
    foreach(var item in @event.Items)
    {
      var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == item.ProductId)
        ?? throw new NullReferenceException();

      product.AddStock(item.Quantity);
    }

    await _context.SaveChangesAsync();
  }
}
