using WebUI.Components;
using WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAntDesign();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient("API", (provider, httpClient) =>
{
  httpClient.BaseAddress = new Uri(builder.Configuration["API_ADDRESS"] ?? string.Empty);
});

builder.Services.AddScoped<UserService>();

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
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
