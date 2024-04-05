using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Mediator;

public static class MediatorConfiguration
{
  public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
  {
    var root = typeof(IRequestHandler<>);
    var implements = assembly.GetTypes()
      .Where(e => e.GetInterfaces().Any(d => d.IsGenericType && d.GetGenericTypeDefinition() == root));

    foreach (var implement in implements)
    {
      // get generic parameter
      var extractInterfaceType = implement.GetInterfaces()
        .FirstOrDefault(e => e.GetGenericTypeDefinition() == root)!;

      services.AddScoped(extractInterfaceType, implement);
    }

    services.AddSingleton<IMediator, MediatorContext>();

    return services;
  }
}
