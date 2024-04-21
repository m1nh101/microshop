namespace Common;

public record Result
{
  private Result() { }

  private Result(Summary? summaryError)
  {
    Error = summaryError;
    IsSuccess = Error is null;
  }

  public Summary? Error { get; set; }
  public bool IsSuccess { get; private set; }
  public object? Data { get; init; }

  public static Result Failed(Summary error) => new(error);
  public static Result Failed(string title, params Error[] error) => new() { Error = new Summary(title, error) };
  public static Result ValidateFailed(params Error[] errors) => new() { Error = new Summary("an validate error", errors) };
  public static Result Ok() => new(summaryError: null) { IsSuccess = true };
  public static Result Ok<TData>(TData data) => new(summaryError: null) { Data = data };

  public TData As<TData>()
  {
    if (Data is null)
      throw new InvalidCastException($"casting type from null data is not valid");

    return (TData)Data;
  }
}
