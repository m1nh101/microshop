namespace API.Contract.Products.Requests;

public record CreateProductRequest(
  string Name,
  double Price,
  string PictureUri,
  string BrandId,
  string Collection,
  string Material,
  CategoryRequest Categories,
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

public record CategoryRequest(
  string[] Add,
  string[] Remove);