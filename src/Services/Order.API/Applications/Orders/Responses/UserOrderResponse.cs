using Order.API.Infrastructure.Entities;

namespace Order.API.Applications.Orders.Responses;

public record UserOrderResponse(
  string Id,
  double TotalPayment,
  string ShippingAddress,
  OrderStatus Status,
  DateTime CreateAt,
  IEnumerable<OrderItem> Items);