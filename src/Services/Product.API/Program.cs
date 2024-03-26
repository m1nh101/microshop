using Auth;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Product.API.HostedServices;
using Product.API.Infrastructure.Database;
using Product.API.RPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJwt(builder.Configuration);
builder.Services.AddFastEndpoints().SwaggerDocument();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapGrpcService<ProductRpcService>();

app.UseHttpsRedirection();

app.UseAuth();

app.UseFastEndpoints();

app.Run();