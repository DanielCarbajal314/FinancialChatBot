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
        IRabbitMessageService _rabbitMessageService;
        IStooqClient _stooqClient;

        public BackgroundStockQueryService(IRabbitMessageService rabbitMessageService, IStooqClient stooqClient) {
            this._rabbitMessageService = rabbitMessageService;
            this._stooqClient = stooqClient;
            _rabbitMessageService.InitStockQueryEvent();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._rabbitMessageService.SubscribeToStockQueryEvent(async stockQuery =>
            {
                Console.WriteLine($"Recieved to query : {stockQuery.StockCode}");
                try
                {
                    var result = await this._stooqClient.QueryStock(stockQuery.StockCode);
                    this._rabbitMessageService.PublishStockQueuedResponst(new DTO.StockQueryResult()
                    {
                        WasSuccessfull = true,
                        Message = $"The stock price is {result.Close}"
                    });
                }
                catch
                {
                    this._rabbitMessageService.PublishStockQueuedResponst(new DTO.StockQueryResult()
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
            this._rabbitMessageService.Dispose();
            base.Dispose();
        }
    }
}
