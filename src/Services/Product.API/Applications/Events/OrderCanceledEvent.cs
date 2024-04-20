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

      product.UpdateUnits(unitToUpdate, true);
    }

    await _context.SaveChangesAsync();
  }
}
