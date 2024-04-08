using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;

namespace Client.Admin.Services;

public enum HttpMethod
{
  GET,
  POST,
  PUT,
  PATCH,
  DELETE,
}

public abstract class BaseService
{
  protected readonly HttpClient Client;

  protected BaseService(HttpClient client)
  {
    Client = client;
  }

  private Task<HttpResponseMessage> SendRequest(string uri, StringContent? payload = null, HttpMethod httpMethod = HttpMethod.GET)
  {
    return httpMethod switch
    {
      HttpMethod.POST => Client.PostAsync(uri, payload),
      HttpMethod.DELETE => Client.DeleteAsync(uri),
      HttpMethod.GET => Client.GetAsync(uri),
      HttpMethod.PUT => Client.PutAsync(uri, payload),
      _ => throw new NotImplementedException()
    };
  }

  protected async Task<Result> MakeRequest<T>(string uri, object req, HttpMethod httpMethod)
    where T : class
  {
    var payload = new StringContent(
      content: JsonConvert.SerializeObject(req),
      encoding: Encoding.UTF8,
      mediaType: "application/json");
    var httpResponse = await SendRequest(uri, payload, httpMethod);
    var rawDataResponse = await httpResponse.Content.ReadAsStringAsync();

    return JsonConvert.DeserializeObject<Result>(rawDataResponse)!;
  }

  protected async Task<Result> MakeRequest(string uri, HttpMethod httpMethod)
  {
    var httpResponse = await SendRequest(uri, httpMethod: httpMethod);
    var rawDataResponse = await httpResponse.Content.ReadAsStringAsync();

    return JsonConvert.DeserializeObject<Result>(rawDataResponse)!;
  }

  //protected async Task<bool> MakeRequest(string uri, HttpMethod httpMethod)
  //{
  //  var httpResponse = await SendRequest(uri, httpMethod: httpMethod);

  //  return httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent;
  //}

  protected void SetCredential(string accessToken)
  {
    Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
  }

  protected string ConvertObjToUri(object obj)
  {
    string jsonString = JsonConvert.SerializeObject(obj);
    var jsonObject = JObject.Parse(jsonString);
    var properties = jsonObject
        .Properties()
        .Where(p => p.Value.Type != JTokenType.Null)
        .Select(p =>
            $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.Value.ToString())}");
    return string.Join("&", properties);
  }
}
