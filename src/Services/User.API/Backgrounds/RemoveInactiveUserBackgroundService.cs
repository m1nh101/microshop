
using Microsoft.EntityFrameworkCore;
using User.API.Infrastructure.Database;

namespace User.API.Backgrounds;

public class RemoveInactiveUserBackgroundService : BackgroundService
{
  private readonly UserDbContext _context;

  public RemoveInactiveUserBackgroundService(UserDbContext context)
  {
    _context = context;
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
    var inactiveUsers = await _context.Users
      .Where(e => !e.IsConfirm)
      .ToListAsync(stoppingToken);
    foreach (var user in inactiveUsers)
    {
      if (DateTime.Now.Subtract(user.CreateAt).Days > 7)
        _context.Remove(user);
    }

    await _context.SaveChangesAsync(stoppingToken);
  }
}
