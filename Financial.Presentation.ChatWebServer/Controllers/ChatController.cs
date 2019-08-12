using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financial.Infrastructure.MessageQueu;
using Financial.Presentation.ChatWebServer.Controllers.Shared;
using Financial.Presentation.ChatWebServer.Hubs;
using Financial.Services.Interfaces.Handlers;
using Financial.Services.Interfaces.Requests.Command;
using Financial.Services.Interfaces.Requests.Query;
using Financial.Services.Interfaces.Responses;
using Financial.Services.Interfaces.Responses.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Financial.Presentation.ChatWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseAPIController
    {
        IChatHandler _chatHandler;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public ChatController
        (
            IChatHandler chatHandler,
            IHubContext<ChatHub> chatHubContext
        )
        {
            this._chatHandler = chatHandler;
            this._chatHubContext = chatHubContext;
        }

        [HttpGet]
        [Route("GetLastMessages")]
        public async Task<IEnumerable<SentMessage>> GetLastMessages()
        {
            return await this._chatHandler.GetLastMessages(50);
        }

        [HttpPost]
        [Route("SendMessage")]
        public async Task<SentMessage> SendMessage(SendChatMessageCommand command)
        {
            var message = await this._chatHandler.SendMessage(command, "Daniel");
            await this._chatHubContext.Clients.All.SendAsync("UserSentMessage", message);
            return message;
        }

        [HttpGet]
        [Route("GetStock")]
        public async Task<StockQueryQueuedResult> GetStock([FromQuery]StockQuery query)
        {
            return await this._chatHandler.GetStock(query);
        }        
    }
}