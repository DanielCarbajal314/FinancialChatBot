using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Financial.Presentation.ChatWebServer.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ChatMessageReceived");
        }
    }
}
