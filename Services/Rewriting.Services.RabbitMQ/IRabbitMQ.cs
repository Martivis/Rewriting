using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.RabbitMQ;

public delegate Task OnDataRecieveAction<T>(T data);

public interface IRabbitMQ
{
    Task Subscribe<T>(string queueName, OnDataRecieveAction<T> onRecieve);
    Task PushAsync<T>(string queueName, T data);
}
