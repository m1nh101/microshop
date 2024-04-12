using Redis.OM;
using Redis.OM.Contracts;
using User.API.Application.CachingModels;

namespace User.API.Backgrounds;

public sealed class RedisIndexService : IHostedService
{
  private readonly IRedisConnectionProvider _provider;

  public RedisIndexService(IRedisConnectionProvider provider)
  {
    _provider = provider;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    await _provider.Connection.CreateIndexAsync(typeof(UserToken));
    await _provider.Connection.CreateIndexAsync(typeof(UserCredential));
    await _provider.Connection.CreateIndexAsync(typeof(UserConfirmation));
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
