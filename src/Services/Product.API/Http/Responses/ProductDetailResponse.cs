namespace Product.API.Http.Responses;

public record ProductDetailResponse(
  string Id,
  string Name,
  double Price,
  int AvailableStock,
  string PictureUri,
  string Description);
