using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace WebUI.Services;

public abstract class BaseService
{
  protected readonly HttpClient Client;
  protected readonly ProtectedLocalStorage Storage;

  public BaseService(IHttpClientFactory factory, ProtectedLocalStorage storage)
  {
    Client = factory.CreateClient("API");
    Storage = storage;
  }
}
