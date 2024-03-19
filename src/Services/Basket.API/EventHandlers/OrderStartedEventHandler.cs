using Basket.API.Events;
using Basket.API.Repositories;
using FastEndpoints;

namespace Basket.API.EventHandlers;

public sealed class OrderStartedEventHandler : ICommandHandler<OrderStartedEvent>
{
  private readonly IBasketRepository _repository;

  public OrderStartedEventHandler(IBasketRepository repository)
  {
    _repository = repository;
  }

  public async Task ExecuteAsync(OrderStartedEvent command, CancellationToken ct)
  {
    await _repository.RemoveBasket(command.UserId);
  }
}
