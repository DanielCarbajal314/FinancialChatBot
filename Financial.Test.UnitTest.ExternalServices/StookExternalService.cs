using Financial.Infrastructure.ExternalServices.StooqClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;

namespace Financial.Test.UnitTest.ExternalServices
{
    [TestClass]
    public class StookExternalService
    {
        [TestMethod]
        public void StooqClientQuery()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
            StooqClient stooqClient = new StooqClient(httpClientFactoryMock.Object);
            var result = stooqClient.QueryStock("aapl.us").Result;
            Assert.AreEqual(result.Symbol, "AAPL.US","The Stock information Symbol is Wrong");
        }
    }
}
