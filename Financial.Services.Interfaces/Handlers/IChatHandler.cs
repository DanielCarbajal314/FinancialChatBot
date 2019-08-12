using Financial.Services.Interfaces.Requests.Command;
using Financial.Services.Interfaces.Requests.Query;
using Financial.Services.Interfaces.Responses;
using Financial.Services.Interfaces.Responses.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Services.Interfaces.Handlers
{
    public interface IChatHandler
    {
        Task<IEnumerable<SentMessage>> GetLastMessages(int numberOfMessagesToList);
        Task<SentMessage> SendMessage(SendChatMessageCommand command, string userName);
        Task<StockQueryQueuedResult> GetStock(StockQuery query);
    }
}
