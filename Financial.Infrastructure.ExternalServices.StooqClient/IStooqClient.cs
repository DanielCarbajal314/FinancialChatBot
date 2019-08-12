using Financial.Infrastructure.ExternalServices.StooqClient.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Infrastructure.ExternalServices.StooqClient
{
    public interface IStooqClient
    {
        Stock QueryStock(string stockName); 
    }
}
