using Common;
using Newtonsoft.Json;
using WebUI.Models;

namespace WebUI.Services;

public sealed class UserService : BaseService
{
  public UserService(IHttpClientFactory factory, IHttpContextAccessor http)
    : base(factory, http)
  {
  }

  public async Task<Result<AuthenticateResponse>> Authenticate(AuthenticateRequest request)
  {
    var response = await Client.PostAsJsonAsync("/user-api/users/auth", request);
    var result = await response.Content.ReadAsStringAsync();

    return JsonConvert.DeserializeObject<Result<AuthenticateResponse>>(result)!;
  }
}
