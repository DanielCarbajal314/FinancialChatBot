using Financial.Domain.Entities;
using Financial.Services.Interfaces.Requests.Command;
using Financial.Services.Interfaces.Responses.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Services.EFImplementation.Mappers
{
    internal static class ChatMessageExtensions
    {
        internal static SentMessage ToDTO(this ChatMessage chatMessage)
        {
            return new SentMessage()
            {
                Id = chatMessage.Id,
                Message = chatMessage.Message,
                SendDate = chatMessage.SendDate,
                Username = chatMessage.Username
            };
        }

        internal static ChatMessage ToEntity(this SendChatMessageCommand sendChatMessageCommand, string userName)
        {
            return new ChatMessage()
            {
                Message = sendChatMessageCommand.Message,
                SendDate = DateTime.UtcNow,
                Username = userName
            };
        }
    }
}
