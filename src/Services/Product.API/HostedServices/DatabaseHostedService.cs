
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
    Console.WriteLine($"Start migration to database at {DateTime.Now:yyyy/MM/dd HH:mm}");

    await _migrator.Migrate().ContinueWith(_ => StopAsync(cancellationToken));
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine($"Complete migration to database at {DateTime.Now:yyyy/MM/dd HH:mm}");
    return Task.CompletedTask;
  }
}
