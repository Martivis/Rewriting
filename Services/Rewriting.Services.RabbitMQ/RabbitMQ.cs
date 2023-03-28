using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace Rewriting.Services.RabbitMQ;

internal class RabbitMQ : IRabbitMQ, IDisposable
{
    private readonly RabbitMQSettings _settings;

    private readonly object _lock = new();
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQ(RabbitMQSettings settings)
    {
        _settings = settings;
    }

    public void Dispose()
    {
        _connection?.Dispose();
        _channel?.Dispose();
    }

    public async Task PushAsync<T>(string queueName, T data)
    {
        Connect();
        DeclareQueue(queueName);

        var json = JsonSerializer.Serialize<object>(data);
        var message = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(exchange: string.Empty,
                             queueName,
                             basicProperties: null,
                             message);
    }

    public async Task Subscribe<T>(string queueName, OnDataRecieveAction<T> onRecieve)
    {
        Connect();
        DeclareQueue(queueName);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (_, eventArgs) =>
        {
            try
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var obj = JsonSerializer.Deserialize<T>(message ?? "");

                await onRecieve(obj!);
                _channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception)
            {
                _channel.BasicNack(eventArgs.DeliveryTag, multiple: false, requeue: false);
            }
        };

        _channel.BasicConsume(queueName, autoAck: false, consumer);
    }

    private void DeclareQueue(string queueName)
    {
        _channel.QueueDeclare(queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
    }

    private void Connect()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_settings.Uri),
            UserName = _settings.UserName,
            Password = _settings.Password,
        };

        if (_connection?.IsOpen ?? false)
            return;

        lock (_lock)
        {
            var attemptsCount = 0;
            bool isConnected = false;
            while (attemptsCount < 15 && !isConnected)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    isConnected = true;
                }
                catch (BrokerUnreachableException)
                {
                    Task.Delay(1000).Wait();
                    attemptsCount++;
                }
            }
        }

    }
}