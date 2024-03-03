
using Product.API.Infrastructure.Database;

namespace Product.API.HostedServices;

public class DatabaseHostedService : IHostedService
{
  private readonly DatabaseMigrator _migrator;

  public DatabaseHostedService(DatabaseMigrator migrator)
  {
    _migrator = migrator;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    await _migrator.Migrate();
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
