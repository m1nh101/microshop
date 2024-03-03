using Common;

namespace Product.API.Infrastructure.Entities;

public class ProductItem
{
  private ProductItem() { }

  public ProductItem(
    string name,
    int availableStock,
    double price,
    string pictureUri,
    string brandId,
    string typeId,
    string description = "")
  {
    Id = Guid.NewGuid().ToString();
    Name = name;
    AvailableStock = availableStock;
    Price = price;
    PictureUri = pictureUri;
    BrandId = brandId;
    TypeId = typeId;
    Description = description;
    IsAvailable = true;
  }

  public ProductItem(
    string name,
    int availableStock,
    double price,
    string pictureUri,
    ProductBrand brand,
    ProductType type,
    string description = "")
  {
    Id = Guid.NewGuid().ToString();
    Name = name;
    AvailableStock = availableStock;
    Price = price;
    PictureUri = pictureUri;
    Brand = brand;
    Type = type;
    Description = description;
    IsAvailable = true;
  }

  public string Id { get; private set; } = string.Empty;
  public string Name { get; private set; } = string.Empty;
  public int AvailableStock { get; private set; }
  public string Description { get; private set; } = string.Empty;
  public double Price { get; private set; }
  public string PictureUri { get; private set; } = string.Empty;

  public bool IsAvailable { get; private set; }

  public void UpdateAvailableStatus(bool isAvailable) => IsAvailable = isAvailable;

  public void Update(ProductItem product)
  {
    Name = product.Name;
    AvailableStock = product.AvailableStock;
    Price = product.Price;
    PictureUri = product.PictureUri;
    BrandId = product.BrandId;
    TypeId = product.TypeId;
    Description = product.Description;
  }

  public string BrandId { get; private set; } = string.Empty;
  public virtual ProductBrand Brand { get; private set; } = null!;

  public string TypeId { get; private set; } = string.Empty;
  public virtual ProductType Type { get; private set; } = null!;

  public Error AddStock(int quantity)
  {
    if (quantity < 0)
      return Errors.PositiveStock;

    AvailableStock += quantity;

    return Error.None;
  }

  public Error RemoveStock(int quantity)
  {
    if (quantity < 0)
      return Errors.PositiveStock;

    if (quantity > AvailableStock)
      return Errors.MaxAvailableStockAllow;

    AvailableStock -= quantity;

    return Error.None;
  }
}
