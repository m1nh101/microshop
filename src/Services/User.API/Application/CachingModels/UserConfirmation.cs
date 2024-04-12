using Redis.OM.Modeling;

namespace User.API.Application.CachingModels;

public class UserConfirmation
{
  [RedisIdField]
  public string Id { get; set; } = string.Empty;
  [Searchable]
  public string ConfirmationCode { get; set; } = string.Empty;
  [Searchable]
  public string Token { get; set; } = string.Empty;
}
