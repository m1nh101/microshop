using Order.API.Infrastructure.Entities;

namespace Order.API.Http.Requests;

public record CreateOrderRequest(
  ShippingAddress ShippingAddress);
