using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace WebUI.Services;

public abstract class BaseService
{
  protected readonly ProtectedLocalStorage Storage;
  protected readonly HttpClient Client;

  public BaseService(
    IHttpClientFactory factory,
    ProtectedLocalStorage storage)
  {
    Client = factory.CreateClient("API");
    Storage = storage;
  }

  protected async Task EnableAuthorizeRequest()
  {
    var getAccessToken = await Storage.GetAsync<string>("access_token");
    if (getAccessToken.Success && !string.IsNullOrEmpty(getAccessToken.Value))
    {
      Client.DefaultRequestHeaders.Add("Authorization", getAccessToken.Value);
      return;
    }

    throw new UnauthorizedAccessException();
  }
}
