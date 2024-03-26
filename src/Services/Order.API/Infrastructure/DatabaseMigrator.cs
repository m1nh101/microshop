using Microsoft.EntityFrameworkCore;

namespace Order.API.Infrastructure;

public sealed class DatabaseMigrator
{
  private readonly IServiceProvider _provider;

  public DatabaseMigrator(IServiceProvider provider)
  {
    _provider = provider;
  }

  public async Task Migrate()
  {
    using var scope = _provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

    if (pendingMigrations is not null)
      await context.Database.MigrateAsync();
  }
}
