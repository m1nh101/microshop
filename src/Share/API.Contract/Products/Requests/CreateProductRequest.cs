namespace API.Contract.Products.Requests;

public record CreateProductRequest(
  string Name,
  int AvailableStock,
  double Price,
  string PictureUri,
  string BrandId,
  string Collection,
  string Material,
  ComplexProperty Categories,
  ProductUnitRequest[] Units,
  string Description = "");

public record EditUnitRequest(
  string Id,
  double Price,
  int Stock);

public record ProductUnitRequest(
  string SizeId,
  string ColorId,
  double Price,
  int Stock);

public record ComplexProperty(
  string[] Add,
  string[] Remove);