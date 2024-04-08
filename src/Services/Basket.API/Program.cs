using Basket.API;
using Basket.API.HostedServices;
using Basket.API.Repositories;
using Basket.API.RPC.Clients;
using Common.Auth;
using Common.Mediator;
using Grpc.Net.Client;
using Redis.OM;
using Redis.OM.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRedisConnectionProvider>(sp =>
{
  var configuration = new RedisConnectionConfiguration
  {
    Host = builder.Configuration["REDIS_SERVER"] ?? builder.Configuration.GetConnectionString("RedisConnection") ?? throw new Exception(),
    Port = 6379
  };

  return new RedisConnectionProvider(configuration);
});

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
  var channel = GrpcChannel.ForAddress("https://product-api:443", option);
  var grpcClient = new ProductRpc.ProductRpcClient(channel);

  return new ProductRpcClient(grpcClient);
});

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddHostedService<RedisIndexHostedService>();

builder.Services.AddMediator(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseHttpsRedirection();

app.UseAuth();

app.UseBasketAPIEndpoint();

app.Run();