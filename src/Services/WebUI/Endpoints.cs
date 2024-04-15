namespace WebUI;

public static class Endpoints
{
  public struct User
  {
    public const string Authentication = "/user-api/users/auth";
    public const string Registration = "/user-api/users/register";
    public const string Logout = "/user-api/users/logout";
  }

  public struct Product
  {
    public const string GetOption = "/product-api/products/options";
    public const string ProductPagination = "/product-api/products";
    public const string ProductDetail = "/product-api/products";
    public const string CreateNew = "/product-api/products";
    public const string Edit = "/product-api/products";
    public const string Delete = "/product-api/products";
  }

  public struct Basket
  {
    public const string GetUser = "/basket-api/baskets";
    public const string AddOrUpdate = "/basket-api/baskets";
    public const string Remove = "/basket-api/baskets/items";
  }
}
