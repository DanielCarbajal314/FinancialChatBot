using Financial.Infrastructure.EFDataPersistance;
using Financial.Infrastructure.MessageQueu;
using Financial.Infrastructure.MessageQueu.DTO;
using Financial.Services.EFImplementation.Mappers;
using Financial.Services.EFImplementation.Shared;
using Financial.Services.Interfaces.Handlers;
using Financial.Services.Interfaces.Requests.Command;
using Financial.Services.Interfaces.Requests.Query;
using Financial.Services.Interfaces.Responses;
using Financial.Services.Interfaces.Responses.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial.Services.EFImplementation
{
    public class ChatHandler : BaseEFHandler, IChatHandler
    {
        private readonly IRabbitStockQuery _rabbitMessageService;

        public ChatHandler(ChatContext db, IRabbitStockQuery rabbitMessageService) : base(db)
        {
            this._rabbitMessageService = rabbitMessageService;
        }

        public async Task<IEnumerable<SentMessage>> GetLastMessages(int numberOfMessagesToList)
        {
            var messages = await this._chatContext.ChatMessages
                                     .OrderByDescending(x => x.SendDate)
                                     .Take(numberOfMessagesToList)
                                     .ToListAsync();

            return messages.Select(x => x.ToDTO());
        }

        public async Task<StockQueryQueuedResult> GetStock(StockQuery query)
        {
            _rabbitMessageService.PublishStockQueuRequest( new StockQueryData() {
                StockCode = query.StockCode
            });
            return new StockQueryQueuedResult()
            {
                Message = "Stock Query was queued",
                Status = "Successfull"
            };
        }

        public async Task<SentMessage> SendMessage(SendChatMessageCommand command,string userName)
        {
            var newChatMessage = command.ToEntity(userName);
            this._chatContext.ChatMessages.Add(newChatMessage);
            await this._chatContext.SaveChangesAsync();
            return newChatMessage.ToDTO();
        }
    }
}
