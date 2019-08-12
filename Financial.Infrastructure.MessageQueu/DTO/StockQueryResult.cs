using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Infrastructure.MessageQueu.DTO
{
    public class StockQueryResult
    {
        public bool WasSuccessfull { get; set; }
        public string Message { get; set; }
    }
}
