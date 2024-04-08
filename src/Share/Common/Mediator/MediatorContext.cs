using Microsoft.Extensions.DependencyInjection;

namespace Common.Mediator;

public class MediatorContext : IMediator
{
  private readonly IServiceProvider _provider;

  public MediatorContext(IServiceProvider provider)
  {
    _provider = provider;
  }

  public Task<Result> Send<TRequest>(TRequest request)
    where TRequest : class
  {
    var scope = _provider.CreateScope();
    var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest>>();
    return handler.Handle(request);
  }
}
