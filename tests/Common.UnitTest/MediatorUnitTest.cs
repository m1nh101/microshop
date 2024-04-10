using Common.Mediator;
using Common.UnitTest.Moq;
using Microsoft.Extensions.DependencyInjection;

namespace Common.UnitTest;

public class MediatorUnitTest
{
  IServiceCollection services = default!;
  IServiceProvider provider = default!;

  [SetUp]
  public void Setup()
  {
    services = new ServiceCollection();
  }

  [Test]
  public void RequestHandlerRegisterToServiceCollectionTest()
  {
    services.AddMediator(typeof(MediatorUnitTest).Assembly);

    var numberOfServicesToRegister = typeof(MediatorUnitTest).Assembly.GetTypes()
      .Count(e => e.GetInterfaces().Any(d => d.IsGenericType && d.GetGenericTypeDefinition() == typeof(IRequestHandler<>)));
    var numberOfServicesRegisterd = services.Count(e => e.ServiceType.IsGenericType && e.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<>));

    Assert.That(numberOfServicesRegisterd, Is.EqualTo(numberOfServicesToRegister));
  }

  [Test]
  public async Task SendRequestTest()
  {
    services.AddMediator(typeof(MediatorUnitTest).Assembly);
    provider = services.BuildServiceProvider();

    var command = new SampleCommand("Test");
    var scope = provider.CreateScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var result = await mediator.Send(command);

    Assert.Multiple(() =>
    {
      Assert.That(result.Data, Is.Not.Null);
      Assert.That(result.As<SampleCommand>().Title, Is.EqualTo("Test"));
    });
  }
}
