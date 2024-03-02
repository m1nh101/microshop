using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.API.Infrastructure.Entities;

namespace User.API.Infrastructure.Database.Configurations;

public sealed class UserRoleEntityConfiguration : IEntityTypeConfiguration<Entities.UserRole>
{
  public void Configure(EntityTypeBuilder<UserRole> builder)
  {
    builder.ToTable("UserRoles");

    builder.HasOne(e => e.User)
      .WithMany(e => e.Roles)
      .HasForeignKey(e => e.UserId);

    builder.HasOne(e => e.Role)
      .WithMany(e => e.Users)
      .HasForeignKey(e => e.RoleId);

    builder.HasKey(e => new { e.RoleId, e.UserId });
  }
}
