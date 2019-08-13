using CsvHelper;
using Financial.Infrastructure.ExternalServices.StooqClient.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Infrastructure.ExternalServices.StooqClient
{
    public class StooqClient : IStooqClient
    {
        private static string URL = @"https://stooq.com";
        IHttpClientFactory _httpClientFactoty;

        public StooqClient(IHttpClientFactory httpClientFactoty)
        {
            this._httpClientFactoty = httpClientFactoty;
        }

        public async Task<Stock> QueryStock(string stockName)
        {
            var client = this._httpClientFactoty.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, URL + $@"/q/l/?s={ stockName }&f=sd2t2ohlcv&h&e=csv");
            var response = await client.SendAsync(request);
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(stream))
            using (var csv = new CsvReader(streamReader))
            {
                 return csv.GetRecords<Stock>().Single();
            }
        }
    }
}
