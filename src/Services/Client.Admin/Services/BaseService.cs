using Common;
using Newtonsoft.Json;
using System.Text;

namespace Client.Admin.Services;

public abstract class BaseService
{
  protected readonly HttpClient Client;

  protected BaseService(HttpClient client)
  {
    Client = client;
  }

  protected async Task<Result<T>> MakeRequest<T>(object req, string uri)
    where T : class
  {
    var payload = new StringContent(
      content: JsonConvert.SerializeObject(req),
      encoding: Encoding.UTF8,
      mediaType: "application/json");
    var httpResponse = await Client.PostAsync(uri, payload);
    var rawDataResponse = await httpResponse.Content.ReadAsStringAsync();

    return JsonConvert.DeserializeObject<Result<T>>(rawDataResponse)!;
  }

  protected Task<Result<T>> MakeRequest<T>(object req, string uri, string accessToken)
    where T : class
  {
    Client.DefaultRequestHeaders.Add("Authorize", $"Bearer {accessToken}");

    return MakeRequest<T>(req, uri);
  }
}
