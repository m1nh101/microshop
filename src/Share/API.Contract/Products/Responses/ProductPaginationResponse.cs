namespace API.Contract.Products.Responses;

public record ProductPaginationResponse(
  string Id,
  string Name,
  double Price,
  string BrandName,
  string PictureUri,
  string Desciption);