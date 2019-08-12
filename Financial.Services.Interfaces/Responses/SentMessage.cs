using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Services.Interfaces.Responses.Query
{
    public class SentMessage
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
    }
}
