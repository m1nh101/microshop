namespace Common;

public record Summary
{
  public Summary(string title, Error error)
  {
    Title = title;
    Detail = new Error[] { error };
  }

  public Summary(string title, params Error[] errors)
  {
    Title = title;
    Detail = errors;
  }

  public string Title { get; init; }
  public IEnumerable<Error> Detail { get; set; } = [];

  public static readonly string NotFound = "Not Found";
  public static readonly string InternalError = "Internal Error";
  public static readonly string MissingRequirement = "Missing Requirement";
  public static readonly string DuplicateValue = "Duplicate value has exist";
  public static readonly string InvalidPayload = "Invalid payload";
}
