using Client.Admin;
using Client.Admin.Components;
using Common.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<RequestBuilder>(client =>
{
  var host = builder.Configuration["SERVER"] ?? "apigateway";

  client.BaseAddress = new Uri($"https://{host}:7168");
}).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
  ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
}); ;

builder.Services.AddBlazorBootstrap();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuth();

app.UseAntiforgery();

app.UseExternalAuthRoute();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
