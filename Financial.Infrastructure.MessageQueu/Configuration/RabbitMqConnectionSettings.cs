using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Infrastructure.MessageQueu.Configuration
{
    public class RabbitMqConnectionSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public string Uri { get; set; }
        public string StockRequestCalculationQueu { get; set; }
        public string StockRequestCalculationResponseQueu { get; set; }
    }
}
