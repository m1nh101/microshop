using Auth;
using Basket.API.HostedServices;
using Basket.API.Repositories;
using Basket.API.RPC.Clients;
using FastEndpoints;
using Redis.OM;
using Redis.OM.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();

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
  return new ProductRpcClient(() => "https://localhost:7295");
});

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddHostedService<RedisIndexHostedService>();

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