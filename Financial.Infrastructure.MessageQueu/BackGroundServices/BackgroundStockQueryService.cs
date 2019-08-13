using Financial.Infrastructure.ExternalServices.StooqClient;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Financial.Infrastructure.MessageQueu.BackGroundServices
{
    public class BackgroundStockQueryService : BackgroundService
    {
        IRabbitStockQuery _rabbitStockQuery;
        IRabbitStockResponse _rabbitStockResponse;
        IStooqClient _stooqClient;

        public BackgroundStockQueryService
        (
            IRabbitStockQuery rabbitStockQuery,
            IRabbitStockResponse rabbitStockResponse,
            IStooqClient stooqClient
        )
        {
            this._rabbitStockQuery = rabbitStockQuery;
            this._rabbitStockResponse = rabbitStockResponse;
            this._stooqClient = stooqClient;
            _rabbitStockQuery.InitStockQueryEvent();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._rabbitStockQuery.SubscribeToStockQueryEvent(async stockQuery =>
            {
                Console.WriteLine($"Recieved to query : {stockQuery.StockCode}");
                try
                {
                    var result = await this._stooqClient.QueryStock(stockQuery.StockCode);
                    this._rabbitStockResponse.PublishStockQueuedResponse(new DTO.StockQueryResult()
                    {
                        WasSuccessfull = true,
                        Message = $"The stock price is {result.Close}"
                    });
                }
                catch
                {
                    this._rabbitStockResponse.PublishStockQueuedResponse(new DTO.StockQueryResult()
                    {
                        WasSuccessfull = false,
                        Message = "Requested Stock Query Failed"
                    });
                }
            });
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            this._rabbitStockResponse.Dispose();
            this._rabbitStockQuery.Dispose();
            base.Dispose();
        }
    }
}
