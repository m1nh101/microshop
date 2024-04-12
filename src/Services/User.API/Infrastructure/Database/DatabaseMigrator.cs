using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Domain.Entities;

namespace User.API.Infrastructure.Database;

public sealed class DatabaseMigrator
{
  private readonly IServiceProvider _provider;
  private readonly IPasswordGenerator _passwordGenerator;

  public DatabaseMigrator(
    IServiceProvider provider,
    IPasswordGenerator passwordGenerator)
  {
    _provider = provider;
    _passwordGenerator = passwordGenerator;
  }

  public async Task Migrate()
  {
    using var scope = _provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

    if (pendingMigrations is not null)
    {
      await context.Database.MigrateAsync();
      await SeedingData(context);
    }
  }

  async Task SeedingData(UserDbContext context)
  {
    var isEmptySeedData = await context.Roles.AnyAsync();
    if (isEmptySeedData)
      return;

    var roles = new Role[]
    {
      new("admin"),
      new("user"),
      new("staff")
    };

    await context.Roles.AddRangeAsync(roles);

    var admin = new Domain.Entities.User(
      username: "admin",
      name: "admin",
      email: "admin@gmail.com",
      phone: string.Empty,
      password: _passwordGenerator.Generate("M1ng@2002")); // this password will be store at other place later;

    await context.Users.AddAsync(admin);
    await context.SaveChangesAsync();
  }
}
