using Common;
using Common.Contracts;

namespace Product.API.Infrastructure.Entities;

public class ProductItem : IIdentity<string>, ICreateable, IAuditable
{
  private ProductItem() { }

  public ProductItem(
    string name,
    double price,
    string picture,
    ProductBrand brand,
    string material,
    string? collection = null,
    string description = "")
  {
    Id = Guid.NewGuid().ToString();
    Name = name;
    Price = price;
    PictureUri = picture;
    Brand = brand;
    Material = material;
    CollectionId = collection;
    Description = description;
    IsAvailable = true;
  }

  public string Id { get; private set; } = string.Empty;
  public string Name { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public double Price { get; private set; }
  public string PictureUri { get; private set; } = string.Empty;
  public string Material { get; private set; } = string.Empty;

  public bool IsAvailable { get; private set; }

  public void UpdateAvailableStatus(bool isAvailable) => IsAvailable = isAvailable;

  public void Update(ProductItem product)
  {
    Name = product.Name;
    Price = product.Price;
    PictureUri = product.PictureUri;
    BrandId = product.BrandId;
    Description = product.Description;
  }

  public string BrandId { get; private set; } = string.Empty;
  public virtual ProductBrand Brand { get; private set; } = null!;

  public string? CollectionId { get; private set; }
  public virtual ProductCollection? Collection { get; private set; }

  private readonly List<ProductUnit> _units = [];
  public virtual IReadOnlyCollection<ProductUnit> Units => _units.AsReadOnly();

  private readonly List<Category> _categories = [];
  public virtual IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();
  public virtual ICollection<ProductCategory> ProductCategories { get; set; } = null!;

  public DateTime? ModifiedAt { get; set; }

  public string? ModifiedBy { get; set; }

  public DateTime CreatedAt { get; set; }

  public string CreatedBy { get; set; } = string.Empty;

  public ProductItem AssignToCategories(params Category[] categories)
  {
    _categories.AddRange(categories);
    return this;
  }

  public ProductItem RemoveAssignedCategories(params string[] categories)
  {
    foreach(var categoryId in categories)
    {
      var existCategory = _categories.First(e => e.Id == categoryId);
      _categories.Remove(existCategory);
    }

    return this;
  }

  public ProductItem AddUnit(ProductUnit unit)
  {
    _units.Add(unit);
    return this;
  }

  public ProductItem UpdateUnits(IDictionary<string, (double Price, int Stock)> units, out ICollection<Error> errors)
  {
    errors = new List<Error>();

    foreach(var unit in units)
    {
      var unitToUpdate = _units.FirstOrDefault(e => e.Id == unit.Key);
      if(unitToUpdate is null)
      {
        errors.Add(Error.NotFound<ProductUnit>(unit.Key));
        continue;
      }

      unitToUpdate.Update(unit.Value.Price, unit.Value.Stock);
    }

    return this;
  }

  public void UpdateUnits(IDictionary<string, int> units, bool isRestore)
  {
    foreach (var unit in units)
    {
      var unitToUpdate = _units.First(e => e.Id == unit.Key);

      if (isRestore)
        unitToUpdate.AddStock(unit.Value);
      else
        unitToUpdate.ReduceStock(unit.Value);
    }
  }

  public ProductItem RemoveUnit(string[] keys, out ICollection<Error> errors)
  {
    errors = new List<Error>();

    foreach(var key in keys)
    {
      var unitToRemove = _units.FirstOrDefault(e => e.Id == key);
      if (unitToRemove is null)
      {
        errors.Add(Error.NotFound<ProductUnit>(key));
        continue;
      }

      _units.Remove(unitToRemove);
    }

    return this;
  }
}