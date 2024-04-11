namespace Common;

public record Result
{
  public Result(Error? error)
  {
    Errors = error is null ? null : [error];
    IsSuccess = error is null;
  }

  public Result(IEnumerable<Error>? errors)
  {
    Errors = errors;
    IsSuccess = errors is null;
  }

  public IEnumerable<Error>? Errors { get; set; }
  public bool IsSuccess { get; protected set; }
  public object? Data { get; init; }

  public static Result Failed(Error error) => new(error);
  public static Result Failed(IEnumerable<Error> errors) => new(errors);
  public static Result Ok() => new(error: null) { IsSuccess = true };
  public static Result Ok<TData>(TData data) => new(error: null) { Data = data };

  public TData As<TData>()
  {
    if (Data is null)
      throw new InvalidCastException($"casting type from null data is not valid");

    return (TData)Data;
  }
}
