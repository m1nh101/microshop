using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;

namespace Client.Admin;

public enum HttpMethod
{
  GET,
  POST,
  PUT,
  PATCH,
  DELETE,
}

public interface IRequestBuilder
{
  Task<(bool IsSuccess, T Data, IEnumerable<Error>? Errors)> Send<T>() where T : class;
}

public interface INoResponseBuilder
{
  Task Send();
}


public class RequestBuilder : INoResponseBuilder, IRequestBuilder
{
  private readonly HttpClient _client;
  private string _endpoint = string.Empty;
  private bool _noResponse = false;
  private object? _payload;
  private HttpMethod _method;
  private string _token = string.Empty;

  public RequestBuilder(HttpClient client)
  {
    _client = client;
  }

  private Task<HttpResponseMessage> SendRequest(HttpMethod method, object? request = null)
  {
    var payload = new StringContent(
      content: JsonConvert.SerializeObject(request),
      encoding: Encoding.UTF8,
      mediaType: "application/json");

    return method switch
    {
      HttpMethod.POST => _client.PostAsync(_endpoint, payload),
      HttpMethod.DELETE => _client.DeleteAsync(_endpoint),
      HttpMethod.GET => _client.GetAsync(_endpoint),
      HttpMethod.PUT => _client.PutAsync(_endpoint, payload),
      _ => throw new NotImplementedException()
    };
  }

  public RequestBuilder WithEndpoint(string endpoint)
  {
    _endpoint = endpoint;
    return this;
  }

  public RequestBuilder WithParams(object obj)
  {
    string jsonString = JsonConvert.SerializeObject(obj);
    var jsonObject = JObject.Parse(jsonString);
    var properties = jsonObject
        .Properties()
        .Where(p => p.Value.Type != JTokenType.Null)
        .Select(p =>
            $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.Value.ToString())}");
    var paramString = string.Join("&", properties);

    _endpoint += $"?{paramString}";

    return this;
  }

  public RequestBuilder WithPayload(object payload)
  {
    _payload = payload;
    return this;
  }

  public RequestBuilder WithAuthorization(string token)
  {
    _token = token;
    return this;
  }

  public RequestBuilder WithHttpMethod(HttpMethod method)
  {
    _method = method;
    return this;
  }

  public INoResponseBuilder NoResponse()
  {
    _noResponse = true;
    return this;
  }

  public async Task Send()
  {
    var jsonPayload = new StringContent(
      content: JsonConvert.SerializeObject(_payload),
      encoding: Encoding.UTF8,
      mediaType: "application/json");

    if (!string.IsNullOrEmpty(_token))
      _client.DefaultRequestHeaders.Add("Authorization", _token);

    _ =_method switch
    {
      HttpMethod.POST => await _client.PostAsync(_endpoint, jsonPayload),
      HttpMethod.DELETE => await _client.DeleteAsync(_endpoint),
      HttpMethod.GET => await _client.GetAsync(_endpoint),
      HttpMethod.PUT => await _client.PutAsync(_endpoint, jsonPayload),
      _ => throw new NotImplementedException()
    };
  }

  public async Task<(bool IsSuccess, T Data, IEnumerable<Error>? Errors)> Send<T>()
    where T : class
  {
    var jsonPayload = new StringContent(
      content: JsonConvert.SerializeObject(_payload),
      encoding: Encoding.UTF8,
      mediaType: "application/json");

    if(!string.IsNullOrEmpty(_token))
      _client.DefaultRequestHeaders.Add("Authorization", _token);

    var httpResponse = _method switch
    {
      HttpMethod.POST => await _client.PostAsync(_endpoint, jsonPayload),
      HttpMethod.DELETE => await _client.DeleteAsync(_endpoint),
      HttpMethod.GET => await _client.GetAsync(_endpoint),
      HttpMethod.PUT => await _client.PutAsync(_endpoint, jsonPayload),
      _ => throw new NotImplementedException()
    };

    var stringResponse = await httpResponse.Content.ReadAsStringAsync();

    if(httpResponse.IsSuccessStatusCode)
      return new(true, JsonConvert.DeserializeObject<T>(stringResponse)!, null);

    return new(false, default!, JsonConvert.DeserializeObject<IEnumerable<Error>>(stringResponse)!);
  }
}
