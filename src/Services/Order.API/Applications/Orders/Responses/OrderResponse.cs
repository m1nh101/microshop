using Order.API.Infrastructure.Entities;

namespace Order.API.Applications.Orders.Responses;

public record OrderResponse(
  string Id,
  string BuyerId,
  string BuyerName,
  double TotalPayment,
  string ShippingAddress,
  OrderStatus Status,
  DateTime CreateAt,
  IEnumerable<OrderItem> Items);
