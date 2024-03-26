using Order.API.Infrastructure.Entities;

namespace Order.API.Http.Requests;

public sealed record GetOrderRequest(
  string UserId,
  OrderStatus? Status,
  DateTime? Time);
