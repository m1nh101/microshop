using Auth;
using Client.Admin;
using Client.Admin.Components;
using Client.Admin.Services;
using CsvHelper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddHttpClient<UserService>(client =>
{
  client.BaseAddress = new Uri("https://localhost:7168");
});

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
