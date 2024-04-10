using Common.EventBus;
using Order.API.Infrastructure.Entities;

namespace Order.API.Applications.Events;

public sealed record OrderCanceledEvent(
  string OrderId,
  IEnumerable<OrderItem> Items) : IntergratedEvent;
