using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Reflection;

namespace Common.EventBus;

public static class EventBusConfiguration
{
  public static IServiceCollection AddEventBus(this IServiceCollection services, Assembly assembly, IConfiguration configuration)
  {
    RegisterIntegratedEventHandler(services, assembly);
    RegisterDomainEventHandler(services, assembly);

    services.AddSingleton(sp =>
    {
      return new ConnectionFactory
      {
        HostName = configuration["MQ_HOST"],
        Ssl = new SslOption
        {
          CertificateValidationCallback = (sender, cert, chain, policy) => true
        },
        DispatchConsumersAsync = true
      };
    });

    services.AddHostedService<RabbitMQEventBus>();
    services.AddSingleton<IEventBus, RabbitMQEventBus>();
    services.AddSingleton<IDomainEventBus, DomainEventBus>();

    return services;
  }

  private static void RegisterIntegratedEventHandler(IServiceCollection services, Assembly assembly)
  {
    var root = typeof(IEventHandler<>);
    var implements = assembly.GetTypes()
      .Where(e => e.GetInterfaces().Any(d => d.IsGenericType && d.GetGenericTypeDefinition() == root));

    foreach (var implement in implements)
    {
      // get generic parameter
      var extractInterfaceType = implement.GetInterfaces()
        .FirstOrDefault(e => e.GetGenericTypeDefinition() == root)!;
      var eventType = extractInterfaceType.GetGenericArguments()[0];

      services.AddKeyedScoped(
        serviceType: typeof(IEventHandler),
        serviceKey: eventType,
        implementationType: implement);
    }
  }

  private static void RegisterDomainEventHandler(IServiceCollection services, Assembly assembly)
  {
    var root = typeof(IDomainEventHandler<>);
    var implements = assembly.GetTypes()
      .Where(e => e.GetInterfaces().Any(d => d.IsGenericType && d.GetGenericTypeDefinition() == root));

    foreach (var implement in implements)
    {
      // get generic parameter
      var extractInterfaceType = implement.GetInterfaces()
        .FirstOrDefault(e => e.GetGenericTypeDefinition() == root)!;

      services.AddScoped(extractInterfaceType, implement);
    }
  }
}