﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.API.Infrastructure.Database.Configurations;

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<Entities.User>
{
  public void Configure(EntityTypeBuilder<Entities.User> builder)
  {
    builder.ToTable("Users");

    builder.HasKey(e => e.Id);

    builder.Property(e => e.Name)
      .HasMaxLength(512)
      .HasColumnType("nvarchar(512)")
      .IsRequired(true);

    builder.Property(e => e.Phone)
      .HasMaxLength(20)
      .HasColumnType("varchar(20)")
      .IsRequired(true);

    builder.Property(e => e.Email)
      .HasMaxLength(128)
      .HasColumnType("varchar(128)")
      .IsRequired(true);

    builder.Property(e => e.Password)
      .HasMaxLength(512)
      .HasColumnType("varchar(512)")
      .IsRequired(true);

    builder.HasIndex(e => new { e.Email, e.Phone });
  }
}
