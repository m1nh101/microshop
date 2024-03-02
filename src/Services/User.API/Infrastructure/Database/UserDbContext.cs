using Microsoft.EntityFrameworkCore;
using User.API.Infrastructure.Entities;

namespace User.API.Infrastructure.Database;

public class UserDbContext : DbContext
{
  public UserDbContext(DbContextOptions<UserDbContext> options)
    : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
  }

  public virtual DbSet<Entities.User> Users => Set<Entities.User>();
  public virtual DbSet<Role> Roles => Set<Role>();
}
