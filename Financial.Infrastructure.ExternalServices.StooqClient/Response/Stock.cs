using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Infrastructure.ExternalServices.StooqClient.Response
{
    public class Stock
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public int Volume { get; set; }
    }
}
