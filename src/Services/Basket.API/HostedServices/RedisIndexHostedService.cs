
using Basket.API.Models;
using Redis.OM;
using Redis.OM.Contracts;

namespace Basket.API.HostedServices;

public class RedisIndexHostedService : IHostedService
{
  private readonly IRedisConnectionProvider _provider;

  public RedisIndexHostedService(IRedisConnectionProvider provider)
  {
    _provider = provider;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    await _provider.Connection.CreateIndexAsync(typeof(CustomerBasket));
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
