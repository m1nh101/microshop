using Common.Auth;
using Common.EventBus;
using Common.Mediator;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Order.API;
using Order.API.Applications.Contracts;
using Order.API.Backgrounds;
using Order.API.Infrastructure;
using Order.API.RPC.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton(sp =>
{
  return new RunningAssembly
  {
    Assembly = typeof(Program).Assembly,
  };
});

builder.Services.AddDbContext<OrderDbContext>(opt =>
{
  var server = builder.Configuration["DB_SERVER"] ?? "127.0.0.1";
  var connectionString = $"Server={server};Database=OrderDb;UID=sa;PWD=M1ng@2002;Encrypt=False";

  //opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnection"));
  opt.UseSqlServer(connectionString);
});

builder.Services.AddSingleton<IBasketClient>(sp =>
{
  var handler = new HttpClientHandler
  {
    ServerCertificateCustomValidationCallback =
      HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
  };
  var option = new GrpcChannelOptions
  {
    HttpHandler = handler
  };
  var host = builder.Configuration["BASKET_RPC_CLIENT"] ?? throw new NullReferenceException();
  var channel = GrpcChannel.ForAddress(host, option); // use env 
  var grpcClient = new BasketRgpc.BasketRgpcClient(channel);

  return new BasketRpcClient(grpcClient);
});

builder.Services.AddSingleton<DatabaseMigrator>();
builder.Services.AddHostedService<DatabaseMigrateService>();

builder.Services.AddJwt(builder.Configuration);
builder.Services.AddMediator(typeof(Program).Assembly);
builder.Services.AddEventBus(typeof(Program).Assembly, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuth();

app.UseOrderAPIEndpoint();

app.Run();