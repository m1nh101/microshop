namespace Order.API.Infrastructure.Entities;

public enum OrderStatus
{
  WaitConfirm = 0,
  Shipping = 2,
  Completed = 3,
  Canceled = 4
}