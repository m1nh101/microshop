using Auth;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Product.API.HostedServices;
using Product.API.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJwt(builder.Configuration);
builder.Services.AddFastEndpoints().SwaggerDocument();

builder.Services.AddDbContext<ProductDbContext>(opt =>
{
  opt.UseSqlServer(builder.Configuration.GetConnectionString("ProductConnection"));
});

builder.Services.AddSingleton<DatabaseMigrator>();

builder.Services.AddHostedService<DatabaseHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseFastEndpoints();

app.Run();