using Common.Auth;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddYamlFile("config.yml", false, true);

builder.Services.AddReverseProxy()
  .LoadFromConfig(builder.Configuration.GetSection("ReserveProxy"));

builder.Services.AddJwt(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseHttpsRedirection();

app.UseAuth();

app.MapReverseProxy();

app.Run();