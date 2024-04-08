using Common.Auth;
using Microsoft.EntityFrameworkCore;
using Order.API;
using Order.API.Applications.Contracts;
using Order.API.Backgrounds;
using Order.API.Infrastructure;
using Order.API.RPC.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<OrderDbContext>(opt =>
{
  var server = builder.Configuration["DB_SERVER"] ?? "127.0.0.1";
  var connectionString = $"Server={server};Database=OrderDb;UID=sa;PWD=M1ng@2002;Encrypt=False";

  //opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnection"));
  opt.UseSqlServer(connectionString);
});

builder.Services.AddSingleton<DatabaseMigrator>();
builder.Services.AddHostedService<DatabaseMigrateService>();

builder.Services.AddScoped<IBasketClient, BasketRpcClient>();

builder.Services.AddJwt(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuth();

app.UseOrderAPIEndpoint();

app.Run();