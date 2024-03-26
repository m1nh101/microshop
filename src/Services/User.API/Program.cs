using Auth;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Redis.OM;
using Redis.OM.Contracts;
using User.API.Application.Contracts;
using User.API.Application.Helpers;
using User.API.Backgrounds;
using User.API.Infrastructure.Caching;
using User.API.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

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
    Host = builder.Configuration["REDIS_SERVER"] ?? builder.Configuration.GetConnectionString("RedisConnection") ?? throw new NullReferenceException(),
    Port = 6379
  };

  return new RedisConnectionProvider(connection);
});

builder.Services.AddSingleton<IAccessTokenGenerator, TokenGenerator>();
builder.Services.AddSingleton<IRefreshTokenGenerator, TokenGenerator>();
builder.Services.AddSingleton<IPasswordGenerator, PasswordGenerator>();

builder.Services.AddSingleton<DatabaseMigrator>();
builder.Services.AddSingleton<UserTokenCachingService>();

builder.Services.AddHostedService<DatabaseMigrateService>();
builder.Services.AddHostedService<RedisIndexService>();

builder.Services.AddJwt(builder.Configuration);
builder.Services.AddFastEndpoints().SwaggerDocument();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuth();

app.UseFastEndpoints();

app.Run();