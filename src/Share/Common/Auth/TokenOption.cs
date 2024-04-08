namespace Common.Auth;

public sealed record TokenOption
{
  public required string SecretKey { get; set; }
  public required int ExpiredIn { get; set; }
}
