using Order.API.Infrastructure.Entities;

namespace Order.API.Http.Responses;

public record CustomerOrderResponse(
  string Id,
  double TotalPayment,
  OrderStatus Status,
  DateTime CreatedAt,
  IEnumerable<OrderItem> Items);
