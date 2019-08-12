using Financial.Services.Interfaces.Requests.Command;
using Financial.Services.Interfaces.Requests.Query;
using Financial.Services.Interfaces.Responses;
using Financial.Services.Interfaces.Responses.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Services.Interfaces.Handlers
{
    public interface IChatHandler
    {
        IEnumerable<SentMessage> GetLastMessages(int numberOfMessagesToList);
        SentMessage SendMessage(SendChatMessageCommand command, string userName);
        StockQueryQueuedResult GetStock(StockQuery query);
    }
}
