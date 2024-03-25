namespace API.Contract.Products.Responses;

public record ProductDetailResponse(
  string Id,
  string Name,
  double Price,
  int AvailableStock,
  string PictureUri,
  string BrandId,
  string TypeId,
  string Description);
