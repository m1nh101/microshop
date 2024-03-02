namespace Common;

public record Result<T>
  where T : class
{
  public Result(
    ResultType resultType,
    T? data,
    IEnumerable<Error>? errors)
  {
    Errors = errors;
    ResultType = resultType;
    Data = data;
  }

  public IEnumerable<Error>? Errors { get; init; }
  public T? Data { get; init; }
  public ResultType ResultType { get; init; }
  public bool IsSuccess => Errors is null;

  public static Result<T> Ok(T data) => new(ResultType.Ok, data, default);
  public static Result<T> Failed(Error error) => new(ResultType.Failed, default, [error]);
  public static Result<T> Failed(IEnumerable<Error> errors) => new(ResultType.Failed, default, errors);

  public static implicit operator Result<T>(T data) => Ok(data);
  public static implicit operator Result<T>(Error error) => Failed(error);
  public static implicit operator Result<T>(List<Error> errors) => Failed(errors);
}
