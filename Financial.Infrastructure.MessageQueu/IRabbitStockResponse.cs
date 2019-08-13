using System;
using Financial.Infrastructure.MessageQueu.DTO;

namespace Financial.Infrastructure.MessageQueu
{
    public interface IRabbitStockResponse : IDisposable
    {
        void InitStockQueryResponseEvent();
        void PublishStockQueuedResponse(StockQueryResult message);
        void SubscribeToStockQueryResponseEvent(Action<StockQueryResult> action);
    }
}