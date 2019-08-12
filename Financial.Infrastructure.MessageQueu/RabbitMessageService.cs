using Financial.Infrastructure.MessageQueu.Configuration;
using Financial.Infrastructure.MessageQueu.DTO;
using Financial.Infrastructure.MessageQueu.Shared;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Financial.Infrastructure.MessageQueu
{
    public class RabbitMessageService : BaseBroker, IRabbitMessageService
    {
        public RabbitMessageService(IOptions<RabbitMqConnectionSettings> connectionSettingsSnapshot) : base(connectionSettingsSnapshot)
        {

        }

        public void PublishStockQueuRequest(StockQueryData message)
        {
            this.Publish(message, this._connectionSettings.StockRequestCalculationQueu);
        }

        public void PublishStockQueuRequest(StockQueryResult message)
        {
            this.Publish(message, this._connectionSettings.StockRequestCalculationResponseQueu);
        }

        public void SubscribeToStockQueryEvent(Action<StockQueryData> action)
        {
            var consumer = new EventingBasicConsumer(this._channel);
            consumer.Received += (ch, ea) =>
            {
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);
                var stockQuery = JsonConvert.DeserializeObject<StockQueryData>(content);
                action(stockQuery);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
        }

        public void InitStockQueryEvent()
        {
            this._connection = _connectionFactory.CreateConnection();
            this._channel = _connection.CreateModel();
            _channel.ExchangeDeclare("amq.topic", ExchangeType.Topic);
            _channel.QueueDeclare(this._connectionSettings.StockRequestCalculationQueu, false, false, false, null);
            _channel.QueueBind(this._connectionSettings.StockRequestCalculationQueu, "amq.topic", "demo.queue.*", null);
            _channel.BasicQos(0, 1, false);
        }
    }
}
