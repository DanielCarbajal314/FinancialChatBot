using Financial.Infrastructure.MessageQueu;
using Financial.Presentation.ChatWebServer.Hubs;
using Financial.Services.Interfaces.Responses.Query;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Financial.Presentation.ChatWebServer.BackgroundTask
{
    public class StockResponseBackgroundMessageBroker : BackgroundService
    {
        private IRabbitMessageService _rabbitMessageService;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public StockResponseBackgroundMessageBroker
        (
            IRabbitMessageService rabbitMessageService,
            IHubContext<ChatHub> chatHubContext
        )
        {
            this._chatHubContext = chatHubContext;
            this._rabbitMessageService = rabbitMessageService;
            this._rabbitMessageService.InitStockQueryResponseEvent();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._rabbitMessageService.SubscribeToStockQueryResponseEvent(async stockResponse=>{
                Console.WriteLine($"Data arrived {stockResponse.Message}");
                await this._chatHubContext.Clients.All.SendAsync("BotSentMessage", new SentMessage() {
                    Message = stockResponse.Message,
                    SendDate = DateTime.Now,
                    Username = "ChatBotFromMQ"
                });
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
