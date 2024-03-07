using Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace WebUI;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
  private readonly ProtectedLocalStorage _storage;
  private readonly ClaimsPrincipal _anoymous;
  private const string AccessToken = "access_token";
  private const string RefreshToken = "refresh_token";

  public CustomAuthenticationStateProvider(ProtectedLocalStorage storage)
  {
    _storage = storage;
    _anoymous = new ClaimsPrincipal(new ClaimsIdentity());
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    var token = await _storage.GetAsync<string>(AccessToken);

    if (string.IsNullOrEmpty(token.Value))
      return await Task.FromResult(new AuthenticationState(_anoymous));

    var identity = JwtParser.Decode(token.Value);
    var user = new ClaimsPrincipal(identity);

    return await Task.FromResult(new AuthenticationState(user));
  }

  public async Task AuthenticateUser(string token)
  {
    await _storage.SetAsync(AccessToken, token);

    var identity = JwtParser.Decode(token);
    var user = new ClaimsPrincipal(identity);

    var state = new AuthenticationState(user);

    NotifyAuthenticationStateChanged(Task.FromResult(state));
  }
}
