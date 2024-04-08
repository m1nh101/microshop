using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;

namespace Client.Admin.Services;

public class ProductService : BaseService
{
  public ProductService(HttpClient client) : base(client)
  {
  }

  public Task<Result> GetProducts(GetProductPaginationRequest request)
  {
    var queryString = ConvertObjToUri(request);
    return MakeRequest($"/product-api/products?{queryString}", HttpMethod.GET);
  }

  public Task<Result> GetProduct(string productId)
  {
    return MakeRequest($"/product-api/products/{productId}", HttpMethod.GET);
  }

  public Task<Result> CreateProduct(CreateProductRequest request, string accessToken)
  {
    SetCredential(accessToken);
    return MakeRequest<ProductDetailResponse>("/product-api/products", request, HttpMethod.POST);
  }

  public Task<Result> EditProduct(EditProductRequest request, string accessToken)
  {
    SetCredential(accessToken);
    return MakeRequest<ProductDetailResponse>($"/product-api/products/{request.Id}", request, HttpMethod.PUT);
  }

  public Task<Result> GetListOption()
  {
    return MakeRequest("/product-api/products/list-option", HttpMethod.GET);
  }
}
