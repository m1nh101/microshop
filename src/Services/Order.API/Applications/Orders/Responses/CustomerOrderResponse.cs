using Order.API.Infrastructure.Entities;

namespace Order.API.Applications.Orders.Responses;

public record CustomerOrderResponse(
  string Id,
  double TotalPayment,
  OrderStatus Status,
  DateTime CreatedAt,
  IEnumerable<OrderItem> Items);
