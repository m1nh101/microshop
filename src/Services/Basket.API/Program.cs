using Basket.API;
using Basket.API.HostedServices;
using Basket.API.Repositories;
using Basket.API.RPC.Clients;
using Basket.API.RPC.Services;
using Common.Auth;
using Common.EventBus;
using Common.Mediator;
using Grpc.Net.Client;
using Redis.OM;
using Redis.OM.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp =>
{
  return new RunningAssembly
  {
    Assembly = typeof(Program).Assembly,
  };
});

builder.Services.AddSingleton<IRedisConnectionProvider>(sp =>
{
  var configuration = new RedisConnectionConfiguration
  {
    Host = builder.Configuration["REDIS_SERVER"] ?? builder.Configuration.GetConnectionString("RedisConnection") ?? throw new Exception(),
    Port = 6379
  };

  return new RedisConnectionProvider(configuration);
});

builder.Services.AddGrpc();

builder.Services.AddSingleton(sp =>
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
  var channel = GrpcChannel.ForAddress("https://product-api:443", option); //use env
  var grpcClient = new ProductRpc.ProductRpcClient(channel);

  return new ProductRpcClient(grpcClient);
});

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddHostedService<RedisIndexHostedService>();

builder.Services.AddMediator(typeof(Program).Assembly);
builder.Services.AddEventBus(typeof(Program).Assembly, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline

app.MapGrpcService<BasketRgpcService>();

app.UseHttpsRedirection();

app.UseAuth();

app.UseBasketAPIEndpoint();

app.Run();