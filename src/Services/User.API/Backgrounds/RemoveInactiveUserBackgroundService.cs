
using Microsoft.EntityFrameworkCore;
using User.API.Infrastructure.Database;

namespace User.API.Backgrounds;

public class RemoveInactiveUserBackgroundService : BackgroundService
{
  private readonly IServiceProvider _provider;

  public RemoveInactiveUserBackgroundService(IServiceProvider provider)
  {
    _provider = provider;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    using var timer = new PeriodicTimer(TimeSpan.FromDays(7));
    while(await timer.WaitForNextTickAsync(stoppingToken))
    {
      await RunCronJob(stoppingToken);
    }
  }

  async Task RunCronJob(CancellationToken stoppingToken)
  {
    var scope = _provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();

    var inactiveUsers = await context.Users
      .Where(e => !e.IsConfirm)
      .ToListAsync(stoppingToken);
    foreach (var user in inactiveUsers)
    {
      if (DateTime.Now.Subtract(user.CreateAt).Days > 7)
        context.Remove(user);
    }

    await context.SaveChangesAsync(stoppingToken);
  }
}
