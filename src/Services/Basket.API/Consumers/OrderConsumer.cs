using Basket.API.Intergrated.Events;
using FastEndpoints;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Basket.API.Consumers;

public class OrderConsumer : IHostedService
{
  public Task StartAsync(CancellationToken cancellationToken)
  {
    var factory = new ConnectionFactory { HostName = "localhost" };
    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();
    channel.QueueDeclare("order_queue");

    var consumer = new AsyncEventingBasicConsumer(channel);
    consumer.Received += async (model, args) =>
    {
      var body = args.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      var payload = JsonConvert.DeserializeObject<OrderStartedEvent>(message);

      if (payload is not null)
        await payload.ExecuteAsync();
    };

    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
