using Common.EventBus;
using Order.API.Infrastructure.Entities;

namespace Order.API.Applications.Events;

public sealed record OrderCreatedEvent(
  string OrderId,
  string UserId,
  IEnumerable<OrderItem> Items) : IntergratedEvent;