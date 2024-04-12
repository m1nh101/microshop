using Common.Auth;
using Common.EventBus;
using Common.Mail;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Redis.OM;
using Redis.OM.Contracts;
using User.API;
using User.API.Application.Contracts;
using User.API.Application.Helpers;
using User.API.Backgrounds;
using User.API.Infrastructure.Caching;
using User.API.Infrastructure.Database;
using User.API.Infrastructure.Database.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp =>
{
  return new RunningAssembly
  {
    Assembly = typeof(Program).Assembly,
  };
});

builder.Services.AddDbContext<UserDbContext>(opt =>
{
  var server = builder.Configuration["DB_SERVER"] ?? "127.0.0.1";
  var connectionString = $"Server={server};Database=UserDb;UID=sa;PWD=M1ng@2002;Encrypt=False";

  //opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnection"));
  opt.UseSqlServer(connectionString);
});

builder.Services.AddSingleton<IRedisConnectionProvider>(sp =>
{
  var connection = new RedisConnectionConfiguration
  {
    Host = builder.Configuration["REDIS_SERVER"]
      ?? builder.Configuration.GetConnectionString("RedisConnection")
      ?? throw new NullReferenceException(),
    Port = 6379
  };

  return new RedisConnectionProvider(connection);
});

builder.Services.AddSingleton<IAccessTokenGenerator, TokenGenerator>();
builder.Services.AddSingleton<IRefreshTokenGenerator, TokenGenerator>();
builder.Services.AddSingleton<IPasswordGenerator, PasswordGenerator>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<DatabaseMigrator>();
builder.Services.AddSingleton<UserTokenCachingStorage>();
builder.Services.AddSingleton<UserCredentialCachingStorage>();
builder.Services.AddSingleton<IUserConfirmationStorage, UserConfirmationStorage>();

builder.Services.AddHostedService<DatabaseMigrateService>();
builder.Services.AddHostedService<RedisIndexService>();

builder.Services.AddJwt(builder.Configuration);
builder.Services.AddMediator(typeof(Program).Assembly);
builder.Services.AddEventBus(typeof(Program).Assembly, builder.Configuration);
builder.Services.AddAzureMail(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuth();

app.UseUserAPIEndpoint();

app.Run();