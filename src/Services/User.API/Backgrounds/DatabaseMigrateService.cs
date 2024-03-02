
using User.API.Infrastructure.Database;

namespace User.API.Backgrounds;

public class DatabaseMigrateService : IHostedService
{
  private readonly DatabaseMigrator _migrator;

  public DatabaseMigrateService(DatabaseMigrator migrator)
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
