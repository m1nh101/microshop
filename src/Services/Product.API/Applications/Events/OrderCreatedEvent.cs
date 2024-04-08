using Common.EventBus;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Applications.Events;

public record OrderItem(
  string ProductId,
  int Quantity);

public record OrderCreatedEvent(
  string OrderId,
  IEnumerable<OrderItem> Items) : IntergratedEvent;

public sealed class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
  private readonly ProductDbContext _context;

  public OrderCreatedEventHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task Handle(OrderCreatedEvent @event)
  {
    foreach (var item in @event.Items)
    {
      var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == item.ProductId)
        ?? throw new NullReferenceException();

      product.RemoveStock(item.Quantity);
    }

    await _context.SaveChangesAsync();
  }
}
