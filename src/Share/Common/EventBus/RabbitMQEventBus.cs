using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Common.EventBus;

public class RabbitMQEventBus : IHostedService, IEventBus
{
  private readonly ConnectionFactory _connectionFactory;
  private readonly RunningAssembly _runningAssembly;
  private readonly IServiceProvider _serviceProvider;

  public RabbitMQEventBus(
    ConnectionFactory connectionFactory,
    IServiceProvider serviceProvider,
    RunningAssembly runningAssembly)
  {
    _connectionFactory = connectionFactory;
    _serviceProvider = serviceProvider;
    _runningAssembly = runningAssembly;
  }

  public Task Publish<TMessage>(TMessage message)
    where TMessage : IntergratedEvent
  {
    var connection = _connectionFactory.CreateConnection();
    using var channel = connection.CreateModel();
    var routingKey = message.GetType().Name;
    channel.QueueDeclare(routingKey);

    var json = JsonConvert.SerializeObject(message);
    var body = Encoding.UTF8.GetBytes(json);
    channel.BasicPublish(exchange: "",
      routingKey: routingKey,
      mandatory: true,
      body: body);

    return Task.CompletedTask;
  }

  public Task StartAsync(CancellationToken cancellationToken)
  {
    // 
    var root = typeof(IEventHandler<>);
    var implements = _runningAssembly.Assembly
      .GetTypes()
      .Where(e => e.GetInterfaces().Any(d => d.IsGenericType && d.GetGenericTypeDefinition() == root));

    foreach (var implement in implements)
    {
      // get generic parameter
      var extractInterfaceType = implement.GetInterfaces()
        .FirstOrDefault(e => e.GetGenericTypeDefinition() == root)!;

      var eventType = extractInterfaceType.GetGenericArguments()[0]!;
      var routingName = eventType.Name;

      var connection = _connectionFactory.CreateConnection();
      var channel = connection.CreateModel();
      channel.QueueDeclare(routingName);

      var consumer = new AsyncEventingBasicConsumer(channel);
      consumer.Received += async (model, eventArgs) =>
      {
        var body = eventArgs.Body.ToArray();
        var messageString = Encoding.UTF8.GetString(body);
        var message = JsonConvert.DeserializeObject<IntergratedEvent>(messageString) ?? throw new Exception();
        var scope = _serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetKeyedServices<IEventHandler>(extractInterfaceType);

        foreach (var handler in handlers)
          await handler.Handle(message);
      };

      channel.BasicConsume(queue: routingName, autoAck: true, consumer: consumer);
    }

    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
