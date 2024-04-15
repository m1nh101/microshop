using System.Text.Json.Serialization;

namespace WebUI;

public class TokenProvider
{
  public string AccessToken { get; set; } = string.Empty;
  public string RefreshToken { get; set; } = string.Empty;

  [JsonIgnore]
  public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken);
}
