using Common.Auth;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddYamlFile("config.yml", false, true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
  .LoadFromConfig(builder.Configuration.GetSection("ReserveProxy"));

builder.Services.AddJwt(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuth();

app.MapReverseProxy();

app.Run();