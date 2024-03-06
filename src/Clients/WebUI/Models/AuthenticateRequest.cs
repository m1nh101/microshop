namespace WebUI.Models;

public class AuthenticateRequest
{
  public string Username { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}

public record AuthenticateResponse(
  string Id,
  string AccessToken,
  string RefreshToken);
