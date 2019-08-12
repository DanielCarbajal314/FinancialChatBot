using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financial.Services.Interfaces.Handlers;
using Financial.Services.Interfaces.Requests.Command;
using Financial.Services.Interfaces.Requests.Query;
using Financial.Services.Interfaces.Responses;
using Financial.Services.Interfaces.Responses.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financial.Presentation.ChatWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        IChatHandler _chatHandler;

        public ChatController(IChatHandler chatHandler)
        {
            this._chatHandler = chatHandler;
        }

        [HttpGet]
        [Route("GetStock")]
        public IEnumerable<SentMessage> GetLastMessages()
        {
            return this._chatHandler.GetLastMessages(50);
        }

        [HttpPost]
        [Route("SendMessage")]
        public SentMessage SendMessage(SendChatMessageCommand command)
        {
            return this._chatHandler.SendMessage(command,"Daniel");
        }

        [HttpGet]
        [Route("GetStock")]
        public StockQueryQueuedResult GetStock([FromQuery]StockQuery query)
        {
            return this._chatHandler.GetStock(query);
        }        
    }
}