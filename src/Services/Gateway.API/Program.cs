using Common.Auth;
using Gateway.API;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddYamlFile("config.yml", false, true);

builder.Services.AddReverseProxy()
  .LoadFromConfig(builder.Configuration.GetSection("ReserveProxy"));

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddRateLimiter(options =>
{
  options.AddFixedWindowLimiter(Constant.API_RATE_LIMITER, opt =>
  {
    opt.PermitLimit = 5;
    opt.Window = TimeSpan.FromSeconds(5);
    opt.QueueLimit = 5;
    opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuth();

app.MapReverseProxy();

app.Run();