using Common;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using WebUI.Models;

namespace WebUI.Services;

public sealed class UserService : BaseService
{
  public UserService(IHttpClientFactory factory, ProtectedLocalStorage storage)
    : base(factory, storage)
  {
  }

  public async Task<Result<AuthenticateResponse>> Authenticate(AuthenticateRequest request)
  {
    var response = await Client.PostAsJsonAsync("/user-api/users/auth", request);
    var result = await response.Content.ReadAsStringAsync();

    var authenticateResponse = JsonConvert.DeserializeObject<Result<AuthenticateResponse>>(result)!;

    await Storage.SetAsync("access_token", authenticateResponse.Data!.AccessToken);
    await Storage.SetAsync("refresh_token", authenticateResponse.Data!.RefreshToken);

    return authenticateResponse;
  }
}
