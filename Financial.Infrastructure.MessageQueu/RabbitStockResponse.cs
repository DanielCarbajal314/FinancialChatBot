using Financial.Infrastructure.MessageQueu.Configuration;
using Financial.Infrastructure.MessageQueu.DTO;
using Financial.Infrastructure.MessageQueu.Shared;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Infrastructure.MessageQueu
{
    public class RabbitStockResponse : BaseBroker, IRabbitStockResponse
    {
        public RabbitStockResponse(IOptions<RabbitMqConnectionSettings> connectionSettingsSnapshot) : base(connectionSettingsSnapshot)
        {

        }

        public void PublishStockQueuedResponse(StockQueryResult message)
        {
            this.Publish(message, this._connectionSettings.StockRequestCalculationResponseQueu, "amq.fanout");
        }

        public void SubscribeToStockQueryResponseEvent(Action<StockQueryResult> action)
        {
            var consumer = new EventingBasicConsumer(this._channel);
            consumer.Received += (ch, ea) =>
            {
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);
                var stockQuery = JsonConvert.DeserializeObject<StockQueryResult>(content);
                action(stockQuery);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(this._connectionSettings.StockRequestCalculationResponseQueu, false, consumer);
        }

        public void InitStockQueryResponseEvent()
        {
            this._connection = _connectionFactory.CreateConnection();
            this._channel = _connection.CreateModel();
            _channel.ExchangeDeclare("amq.fanout", ExchangeType.Fanout, durable: true);
            _channel.QueueDeclare(this._connectionSettings.StockRequestCalculationResponseQueu, true, false, false, null);
            _channel.QueueBind(this._connectionSettings.StockRequestCalculationResponseQueu, "amq.fanout", this._connectionSettings.StockRequestCalculationResponseQueu, null);
            _channel.BasicQos(0, 1, false);
        }
    }
}
