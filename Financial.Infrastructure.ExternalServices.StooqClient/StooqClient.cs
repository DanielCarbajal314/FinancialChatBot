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
    public class StooqClient
    {
        private static string URL = @"https://stooq.com";
        HttpClient _httpClient;

        public StooqClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<Stock> QueryStock(string stockName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, URL + $@"/q/l/?s={ stockName }&f=sd2t2ohlcv&h&e=csv");
            var response = await this._httpClient.SendAsync(request);
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(stream))
            using (var csv = new CsvReader(streamReader))
            {
                 return csv.GetRecords<Stock>().Single();
            }
        }
    }
}
