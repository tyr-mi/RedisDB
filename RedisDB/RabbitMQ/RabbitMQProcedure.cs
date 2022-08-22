using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RedisDB.RabbitMQ
{
    public class RabbitMQProcedure : IMessageProcedure
    {

        IConnection _connection;

        public RabbitMQProcedure()
        {
            var factory = new ConnectionFactory { HostName = "127.0.0.1" };
            _connection = factory.CreateConnection();
        }

        public void SendMessage<T>(T message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel
                    .QueueDeclare(
                    queue: "orders",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: "", routingKey: "orders", body: body, basicProperties: null);
            }

        }
    }
}
