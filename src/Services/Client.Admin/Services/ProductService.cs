using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;

namespace Client.Admin.Services;

public class ProductService : BaseService
{
  public ProductService(HttpClient client) : base(client)
  {
  }

  public Task<Result<IEnumerable<ProductPaginationResponse>>> GetProducts(GetProductPaginationRequest request)
  {
    var queryString = ConvertObjToUri(request);
    return MakeRequest<IEnumerable<ProductPaginationResponse>>($"/product-api/products?{queryString}", HttpMethod.GET);
  }

  public Task<Result<ProductDetailResponse>> GetProduct(string productId)
  {
    return MakeRequest<ProductDetailResponse>($"/product-api/products/{productId}", HttpMethod.GET);
  }

  public Task<Result<ProductDetailResponse>> CreateProduct(CreateProductRequest request, string accessToken)
  {
    SetCredential(accessToken);
    return MakeRequest<ProductDetailResponse>("/product-api/products", request, HttpMethod.POST);
  }

  public Task<Result<ProductDetailResponse>> EditProduct(EditProductRequest request, string accessToken)
  {
    SetCredential(accessToken);
    return MakeRequest<ProductDetailResponse>($"/product-api/products/{request.Id}", request, HttpMethod.PUT);
  }

  public Task<Result<FilterOptionResponse>> GetListOption()
  {
    return MakeRequest<FilterOptionResponse>("/product-api/products/list-option", HttpMethod.GET);
  }
}
