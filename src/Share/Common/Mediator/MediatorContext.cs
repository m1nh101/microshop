using Microsoft.Extensions.DependencyInjection;

namespace Common.Mediator;

public class MediatorContext : IMediator
{
  private readonly IServiceProvider _provider;

  public MediatorContext(IServiceProvider provider)
  {
    _provider = provider;
  }

  private Task<object> _handlerTask = null!;

  public IMediator Send<TRequest>(TRequest request)
    where TRequest : class
  {
    var scope = _provider.CreateScope();
    var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest>>();
    _handlerTask = handler.Handle(request);
    return this;
  }

  public async Task<TResponse> As<TResponse>()
  {
    var response = await _handlerTask;
    return (TResponse)response;
  }
}
