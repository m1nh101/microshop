namespace Common;

public record Error(
  string Code,
  string Description)
{
  public static readonly Error None = new(string.Empty, string.Empty);
  public static Error NotFound<TEntity>(string key) => new(typeof(TEntity).Name, $"{key} không hợp lệ");
  public static IEnumerable<Error> NotFound<TEntity>(IEnumerable<string> keys) => keys.Select(e => NotFound<TEntity>(e));
}