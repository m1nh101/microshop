namespace Product.API.Infrastructure.Database;

public class ProductCsvRecord
{
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string PictureUri { get; set; } = string.Empty;
  public double Price { get; set; }
  public int AvailableStock { get; set; }
  public bool IsAvailable { get; set; }
}
