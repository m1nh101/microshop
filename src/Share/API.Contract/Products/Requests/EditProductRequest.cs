namespace API.Contract.Products.Requests;

public record EditProductRequest(
  string Id,
  string Name,
  double Price,
  string PictureUri,
  string BrandId,
  string Collection,
  string Material,
  CategoryRequest Categories,
  EditUnit Unit,
  string Description = "");

public record EditUnit(
  ProductUnitRequest[] Add,
  EditUnitRequest[] Edit,
  string[] Remove);