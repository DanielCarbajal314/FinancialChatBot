﻿using Financial.Infrastructure.MessageQueu.Configuration;
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
    public class RabbitStockQuery : BaseBroker, IRabbitStockQuery
    {

        public RabbitStockQuery(IOptions<RabbitMqConnectionSettings> connectionSettingsSnapshot) : base(connectionSettingsSnapshot){}

        public void PublishStockQueuRequest(StockQueryData message)
        {
            this.Publish(message, this._connectionSettings.StockRequestCalculationQueu);
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
            _channel.BasicConsume(this._connectionSettings.StockRequestCalculationQueu, false, consumer);
        }

        public void InitStockQueryEvent()
        {
            this._connection = _connectionFactory.CreateConnection();
            this._channel = _connection.CreateModel();
            _channel.ExchangeDeclare("amq.direct", ExchangeType.Direct,durable:true);
            _channel.QueueDeclare(this._connectionSettings.StockRequestCalculationQueu, true, false, false, null);
            _channel.QueueBind(this._connectionSettings.StockRequestCalculationQueu, "amq.direct", this._connectionSettings.StockRequestCalculationQueu, null);
            _channel.BasicQos(0, 1, false);
        }
    }
}
