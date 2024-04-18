//using API.Contract.Products.Requests;
//using API.Contract.Products.Responses;
//using Common.EventBus;
//using Common.Mediator;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Moq;
//using Product.API.Infrastructure.Database;
//using Product.API.Infrastructure.Entities;

//namespace Product.Test;
//public class ProductIntegrationTest
//{
//  private IServiceCollection _services;
//  private IServiceProvider _provider;
//  private IMediator _mediator;
//  private string brandId = default!;
//  private string typeId = default!;
//  private string productId = default!;

//  [OneTimeSetUp]
//  public void Setup()
//  {
//    _services = new ServiceCollection();
//    //_services.AddScoped(sp =>
//    //{
//    //  var optionBuilder = new DbContextOptionsBuilder<ProductDbContext>();
//    //  optionBuilder.UseInMemoryDatabase("ProductDb");

//    //  return new ProductDbContext(optionBuilder.Options);
//    //});
//    _services.AddMediator(typeof(ProductItem).Assembly);

//    _services.AddSingleton(sp =>
//    {
//      var mock = new Mock<IEventBus>();
//      mock.Setup(e => e.Publish(It.IsAny<IntergratedEvent>())).Returns(Task.CompletedTask);
//      return mock.Object;
//    });

//    _provider = _services.BuildServiceProvider();
//    _mediator = _provider.GetRequiredService<IMediator>();
//  }

//  [Test, Order(1)]
//  public async Task AddSampleDataTest()
//  {
//    var brand = new ProductBrand("Nike");
//    var context = _provider.CreateScope().ServiceProvider.GetRequiredService<ProductDbContext>();

//    await context.Brands.AddAsync(brand);
//    await context.SaveChangesAsync();

//    brandId = brand.Id;

//    Assert.Multiple(() =>
//    {
//      Assert.That(brandId, Is.Not.Null);
//      Assert.That(brandId, Is.Not.Empty);
//    });
//  }

//  [Test, Order(2)]
//  public async Task CreateNewProductTest()
//  {
//    var command = new CreateProductRequest(
//      "Shirt",
//      10,
//      100,
//      "empty",
//      brandId,
//      typeId);
//    var result = await _mediator.Send(command);

//    productId = result.As<ProductDetailResponse>().Id;

//    Assert.Multiple(() =>
//    {
//      Assert.That(result.IsSuccess, Is.True);
//      Assert.That(result.Data, Is.Not.Null);
//      Assert.That(result.As<ProductDetailResponse>().Id, Is.Not.Empty);
//    });
//  }

//  [Test, Order(3)]
//  public async Task GetProductsTest()
//  {
//    var query = new GetProductPaginationRequest() { PageIndex = 0 };
//    var result = await _mediator.Send(query);

//    Assert.Multiple(() =>
//    {
//      Assert.That(result.IsSuccess, Is.True);
//      Assert.That(result.As<IList<ProductPaginationResponse>>(), Has.Count.GreaterThan(0));
//    });
//  }

//  [Test, Order(4)]
//  public async Task GetProductTest()
//  {
//    var query = new GetProductByIdRequest() { Id = productId };
//    var result = await _mediator.Send(query);

//    Assert.Multiple(() =>
//    {
//      Assert.That(result.IsSuccess, Is.True);
//      Assert.That(result.As<ProductDetailResponse>().Id, Is.EqualTo(productId));
//    });
//  }

//  [Test, Order(5)]
//  public async Task EditProductTest()
//  {
//    var command = new EditProductRequest(
//      productId,
//      "Nike Air Force 1",
//      5,
//      100,
//      "empty",
//      brandId,
//      typeId);
//    var result = await _mediator.Send(command);

//    Assert.Multiple(() =>
//    {
//      Assert.That(result.IsSuccess, Is.True);
//      Assert.That(result.As<ProductDetailResponse>().AvailableStock, Is.EqualTo(5));
//      Assert.That(result.As<ProductDetailResponse>().Name, Is.EqualTo("Nike Air Force 1"));
//    });
//  }

//  [Test, Order(7)]
//  public async Task EditNotExistProductTest()
//  {
//    var command = new EditProductRequest(
//      "1892379182",
//      "Nike Air Force 1",
//      5,
//      100,
//      "empty",
//      brandId,
//      typeId);
//    var result = await _mediator.Send(command);

//    Assert.Multiple(() =>
//    {
//      Assert.That(result.IsSuccess, Is.False);
//      Assert.That(result.Errors!.Count(), Is.GreaterThan(0));
//    });
//  }

//  [Test, Order(8)]
//  public async Task RemoveProductTest()
//  {
//    var command = new RemoveProductRequest() { Id = productId };
//    var result = await _mediator.Send(command);

//    Assert.That(result.IsSuccess, Is.True);
//  }

//  [Test, Order(9)]
//  public async Task RemoveNotExistProductTest()
//  {
//    var command = new RemoveProductRequest { Id = "1893271923" };
//    var result = await _mediator.Send(command);

//    Assert.Multiple(() =>
//    {
//      Assert.That(result.IsSuccess, Is.False);
//      Assert.That(result.Errors!.Count(), Is.GreaterThan(0));
//    });
//  }
//}
