namespace WebUI.Services;

public abstract class BaseService
{
  protected readonly HttpClient Client;

  public BaseService(IHttpClientFactory factory, IHttpContextAccessor http)
  {
    Client = factory.CreateClient("API");
    //SettingHttpClient(http);
  }

  private void SettingHttpClient(IHttpContextAccessor http)
  {
    var accessToken = http.HttpContext?.Request.Headers.Authorization.ToString()
      ?? http.HttpContext?.Request.Cookies["access_token"]?.ToString()
      ?? string.Empty;

    Client.DefaultRequestHeaders.Add("Authorization", accessToken);
  }
}
