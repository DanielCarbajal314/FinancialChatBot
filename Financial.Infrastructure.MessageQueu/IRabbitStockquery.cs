using System;
using Financial.Infrastructure.MessageQueu.DTO;

namespace Financial.Infrastructure.MessageQueu
{
    public interface IRabbitStockQuery : IDisposable
    {
        void InitStockQueryEvent();
        void PublishStockQueuRequest(StockQueryData message);
        void SubscribeToStockQueryEvent(Action<StockQueryData> action);
    }
}