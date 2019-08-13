using Financial.Infrastructure.MessageQueu;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial.Presentation.ChatWebServer.BackgroundTask
{
    public class StockResponseBackgroundMessageBroker : BackgroundService
    {
        IRabbitMessageService _rabbitMessageService;

        public StockResponseBackgroundMessageBroker(IRabbitMessageService rabbitMessageService)
        {
            this._rabbitMessageService = rabbitMessageService;
        }
    }
}
