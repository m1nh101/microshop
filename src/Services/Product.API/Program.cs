using Common.Auth;
using Common.EventBus;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API;
using Product.API.HostedServices;
using Product.API.Infrastructure.Database;
using Product.API.RPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp =>
{
  return new RunningAssembly
  {
    Assembly = typeof(Program).Assembly,
  };
});

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddDbContext<ProductDbContext>(opt =>
{
  var server = builder.Configuration["DB_SERVER"] ?? "127.0.0.1";
  var connectionString = $"Server={server};Database=ProductDb;UID=sa;PWD=M1ng@2002;Encrypt=False";

  //opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnection"));
  opt.UseSqlServer(connectionString);
});

builder.Services.AddSingleton<DatabaseMigrator>();

builder.Services.AddHostedService<DatabaseHostedService>();

builder.Services.AddGrpc();

builder.Services.AddMediator(typeof(Program).Assembly);
//builder.Services.AddEventBus(typeof(Program).Assembly, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<ProductRpcService>();

app.UseHttpsRedirection();

app.UseAuth();

app.UseProductAPIEndpoint();

app.Run();