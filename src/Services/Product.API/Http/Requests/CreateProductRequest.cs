namespace Product.API.Http.Requests;

public record CreateProductRequest(
  string Name,
  int AvailableStock,
  double Price,
  string PictureUri,
  string BrandId,
  string TypeId,
  string Description = "");
