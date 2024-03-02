using Microsoft.Extensions.Configuration;

namespace AppSetting;

public static class AppConfiguration
{
  private static readonly IConfiguration _configuration = new ConfigurationBuilder()
      .SetBasePath(typeof(AppConfiguration).Assembly.Location)
      .AddJsonFile("appsettings.json", optional: false)
      .Build();

  public static string GetConnectionString(string key)
  {
    return _configuration.GetConnectionString(key) ?? throw new NullReferenceException();
  }
}
