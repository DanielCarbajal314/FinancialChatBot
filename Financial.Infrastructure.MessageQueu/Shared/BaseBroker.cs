using Financial.Infrastructure.MessageQueu.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Infrastructure.MessageQueu.Shared
{
    public class BaseBroker : IDisposable
    {
        protected readonly RabbitMqConnectionSettings _connectionSettings;
        protected readonly ConnectionFactory _connectionFactory;
        protected IConnection _connection;
        protected IModel _channel;

        public BaseBroker(IOptions<RabbitMqConnectionSettings> connectionSettingsSnapshot)
        {
            this._connectionSettings = connectionSettingsSnapshot.Value;
            this._connectionFactory = new ConnectionFactory
            {
                UserName = this._connectionSettings.Username,
                Password = this._connectionSettings.Password,
                VirtualHost = this._connectionSettings.VirtualHost,
                HostName = this._connectionSettings.HostName,
                Uri = new Uri(this._connectionSettings.Uri)
            };
        }

        public void Dispose()
        {
            if (this._channel != null)
            {
                this._channel.Close();
                this._connection.Close();
            }
        }

        protected void Publish(object message,string queueName, string exchange = "amq.direct")
        {
            using (var conn = this._connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    var jsonPayload = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(jsonPayload);
                    channel.BasicPublish(exchange: exchange,
                        routingKey: queueName,                        
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }
    }
}
