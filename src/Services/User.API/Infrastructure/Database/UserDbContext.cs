using Common.EventBus;
using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Domain.Entities;

namespace User.API.Infrastructure.Database;

public class UserDbContext : DbContext
{
  private readonly IDomainEventBus _eventBus;

  public UserDbContext(DbContextOptions<UserDbContext> options,
    IDomainEventBus eventBus)
    : base(options)
  {
    _eventBus = eventBus;
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    foreach(var entry in ChangeTracker.Entries<DomainEvent>())
    {
      var events = entry.Entity.Events;

      foreach(var @event in events)
      {
        await _eventBus.Publish(@event);
        entry.Entity.RemoveEvent();
      }
    }

    foreach(var entry in ChangeTracker.Entries<IRemoveable>())
    {
      if (entry.State == EntityState.Deleted)
        entry.Entity.Remove();
    }

    return await base.SaveChangesAsync(cancellationToken);
  }

  public virtual DbSet<Domain.Entities.User> Users => Set<Domain.Entities.User>();
  public virtual DbSet<Role> Roles => Set<Role>();
}
