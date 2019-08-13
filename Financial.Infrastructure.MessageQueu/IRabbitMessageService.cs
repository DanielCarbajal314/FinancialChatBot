using System;
using Financial.Infrastructure.MessageQueu.DTO;

namespace Financial.Infrastructure.MessageQueu
{
    public interface IRabbitMessageService : IDisposable
    {
        void InitStockQueryEvent();
        void PublishStockQueuRequest(StockQueryData message);
        void PublishStockQueuedResponst(StockQueryResult message);
        void SubscribeToStockQueryEvent(Action<StockQueryData> action);
    }
}