using Financial.Infrastructure.EFDataPersistance;
using Financial.Services.EFImplementation.Mappers;
using Financial.Services.EFImplementation.Shared;
using Financial.Services.Interfaces.Handlers;
using Financial.Services.Interfaces.Requests.Command;
using Financial.Services.Interfaces.Requests.Query;
using Financial.Services.Interfaces.Responses;
using Financial.Services.Interfaces.Responses.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Financial.Services.EFImplementation
{
    public class ChatHandler : BaseEFHandler, IChatHandler
    {
        public ChatHandler(ChatContext db) : base(db) { }

        public IEnumerable<SentMessage> GetLastMessages(int numberOfMessagesToList)
        {
            return this._chatContext.ChatMessages
                             .OrderBy(x=>x.SendDate)
                             .TakeLast(numberOfMessagesToList)
                             .ToList()
                             .Select(x=>x.ToDTO());
        }

        public StockQueryQueuedResult GetStock(StockQuery query)
        {
            throw new NotImplementedException();
        }

        public SentMessage SendMessage(SendChatMessageCommand command,string userName)
        {
            var newChatMessage = command.ToEntity(userName);
            this._chatContext.ChatMessages.Add(newChatMessage);
            this._chatContext.SaveChanges();
            return newChatMessage.ToDTO();
        }
    }
}
