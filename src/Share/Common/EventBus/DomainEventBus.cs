using Microsoft.Extensions.DependencyInjection;

namespace Common.EventBus;

public class DomainEventBus : IDomainEventBus
{
  private readonly IServiceProvider _provider;

  public DomainEventBus(IServiceProvider provider)
  {
    _provider = provider;
  }

  async Task IDomainEventBus.Publish<TEvent>(TEvent @event)
  {
    var scope = _provider.CreateScope();
    var handler = scope.ServiceProvider.GetRequiredService<IDomainEventHandler<TEvent>>();

    await handler.Handle(@event);
  }
}