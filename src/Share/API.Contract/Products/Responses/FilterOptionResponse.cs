namespace API.Contract.Products.Responses;

public class FilterOptionResponse
{
  public FilterOptionResponse(
    IEnumerable<SelectOption> brandOptions)
  {
    BrandOptions = brandOptions;
  }
  public IEnumerable<SelectOption> BrandOptions { get; }
}

public class SelectOption
{
  public string Value { get; set; } = string.Empty;
  public string Label { get; set; } = string.Empty;
}
