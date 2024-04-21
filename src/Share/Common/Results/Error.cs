namespace Common;

public record Error(
  string Field,
  string Description)
{
  public static readonly Error None = new(string.Empty, string.Empty);
  public static Error NotFound<TEntity>(string key) => new(typeof(TEntity).Name, $"{key} không hợp lệ");
  public static IEnumerable<Error> NotFound<TEntity>(IEnumerable<string> keys) => keys.Select(e => NotFound<TEntity>(e));
  public static Error[]? Verify(Error[] errors)
  {
    var result = errors.Where(e => !e.Equals(None)).ToArray();

    if (result.Length == 0)
      return null;

    return result;
  }
}