using Basket.API.Repositories;
using Common.EventBus;

namespace Basket.API.Applications.Events;


public sealed record OrderCreatedEvent(
  string OrderId,
  string UserId) : IntergratedEvent;

public sealed class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
  private readonly IBasketRepository _repository;

  public OrderCreatedEventHandler(IBasketRepository repository)
  {
    _repository = repository;
  }

  public async Task Handle(OrderCreatedEvent @event)
  {
    await _repository.RemoveBasket(@event.UserId);
  }
}