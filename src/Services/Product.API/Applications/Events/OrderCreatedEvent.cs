using Common.EventBus;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Applications.Events;

public record OrderItem(
  string ProductId,
  string UnitId,
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
    var groupUnitByProduct = @event.Items.GroupBy(e => e.ProductId)
      .Select(e => new
      {
        ProductId = e.Key,
        Units = e.Select(d => new
        {
          d.UnitId,
          d.Quantity,
        })
      }).ToList();

    foreach (var item in groupUnitByProduct)
    {
      var product = await _context.Products
        .Include(e => e.Units)
        .FirstOrDefaultAsync(e => e.Id == item.ProductId)
        ?? throw new NullReferenceException();

      var unitToUpdate = item.Units
        .ToDictionary(e => e.UnitId, e => e.Quantity);

      product.UpdateUnits(unitToUpdate, false);
    }

    await _context.SaveChangesAsync();
  }
}