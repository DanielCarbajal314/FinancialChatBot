using Financial.Domain.Entities.Shared;
using System;

namespace Financial.Domain.Entities
{
    public class ChatMessage : BaseEntity
    {
        public DateTime SendDate { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
