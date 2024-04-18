//using Common;
//using Product.API.Infrastructure.Entities;

//namespace Product.Test;

//public class ProductItemUnitTest
//{
//  readonly string name = "Shirt";
//  readonly int stock = 10;
//  readonly double price = 100.52;
//  readonly string picture = "empty";
//  readonly string brandId = "brandId";
//  readonly string typeId = "typeId";
//  readonly string description = "description";
//  ProductItem product;

//  [SetUp]
//  public void Setup()
//  {
//    product = new ProductItem(
//      name,
//      stock,
//      price,
//      picture,
//      brandId,
//      description);
//  }

//  [Test]
//  public void ConstructorTest()
//  {
//    var product = new ProductItem(
//      name,
//      stock,
//      price,
//      picture,
//      brandId,
//      description);

//    Assert.Multiple(() =>
//    {
//      Assert.That(product, Is.Not.Null);
//      Assert.That(product.Brand, Is.Null);
//      Assert.That(product.Name, Is.EqualTo(name));
//      Assert.That(product.AvailableStock, Is.EqualTo(stock));
//      Assert.That(product.Price, Is.EqualTo(price));
//      Assert.That(product.PictureUri, Is.EqualTo(picture));
//      Assert.That(product.BrandId, Is.EqualTo(brandId));
//      Assert.That(product.Description, Is.EqualTo(description));
//    });
//  }

//  [Test]
//  public void AddStockSuccessTest()
//  {
//    var additionStock = 5;
    
//    var error = product.AddStock(additionStock);

//    Assert.Multiple(() =>
//    {
//      Assert.That(error, Is.EqualTo(Error.None));
//      Assert.That(product.AvailableStock, Is.EqualTo(stock + additionStock));
//    });
//  }

//  [Test]
//  public void AddStockFailedTest()
//  {
//    var additionStock = -1;
//    var error = product.RemoveStock(additionStock);

//    Assert.Multiple(() =>
//    {
//      Assert.That(error, Is.EqualTo(new Error("Catalog.Stock", "Available stock cannot be positive")));
//      Assert.That(product.AvailableStock, Is.EqualTo(stock));
//    });
//  }

//  [Test]
//  public void RemoveStockSuccessTest()
//  {
//    var removeStock = 5;
//    var error = product.RemoveStock(removeStock);
    
//    Assert.Multiple(() =>
//    {
//      Assert.That(product.AvailableStock, Is.EqualTo(stock - removeStock));
//      Assert.That(error, Is.EqualTo(Error.None));
//    });
//  }

//  [Test]
//  public void RemovePositiveStockTest()
//  {
//    var removeStock = -1;
//    var error = product.RemoveStock(removeStock);

//    Assert.Multiple(() =>
//    {
//      Assert.That(product.AvailableStock, Is.EqualTo(stock));
//      Assert.That(error, Is.EqualTo(new Error("Catalog.Stock", "Available stock cannot be positive")));
//    });
//  }

//  [Test]
//  public void RemoveLargerAmountThanStockTest()
//  {
//    var removeStock = 20;
//    var error = product.RemoveStock(removeStock);

//    Assert.Multiple(() =>
//    {
//      Assert.That(product.AvailableStock, Is.EqualTo(stock));
//      Assert.That(error, Is.EqualTo(new Error("Catalog.Stock", "Stock doesn't enough to order")));
//    });
//  }
//}