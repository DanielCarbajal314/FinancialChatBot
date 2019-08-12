using Financial.Infrastructure.MessageQueu.DTO;

namespace Financial.Infrastructure.MessageQueu
{
    public interface IRabbitMessageService
    {
        void PublishStockQueuRequest(StockQueryData message);
        void PublishStockQueuRequest(StockQueryResult message);
    }
}