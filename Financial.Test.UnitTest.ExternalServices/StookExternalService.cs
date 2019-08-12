using Financial.Infrastructure.ExternalServices.StooqClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace Financial.Test.UnitTest.ExternalServices
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void StooqClientQuery()
        {
            StooqClient stooqClient = new StooqClient(new HttpClient());
            var result = stooqClient.QueryStock("aapl.us").Result;
            Assert.AreEqual(result.Symbol, "AAPL.US","The Stock information Symbol is Wrong");
        }
    }
}
