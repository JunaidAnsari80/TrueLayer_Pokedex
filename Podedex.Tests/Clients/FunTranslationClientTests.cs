using Moq;
using Moq.Protected;
using NUnit.Framework;
using Podedex.Services.Clients;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Podedex.Tests.Clients
{
    [TestFixture]
    public class FunTranslationClientTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<DelegatingHandler> _clientHandlerMock;
        private string _content = "{\"success\": {\"total\": 0},\"contents\": {\"translated\": \"translated\",\"text\": \"translate\",\"translation\": \"translation\"}}";

        [SetUp]
        public void SetUp()
        {
            _clientHandlerMock = new Mock<DelegatingHandler>();

            _clientHandlerMock.As<IDisposable>().Setup(s => s.Dispose());

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            var httpClient = new HttpClient(_clientHandlerMock.Object);

            httpClient.BaseAddress = new Uri("http://www.google.com");

            _mockHttpClientFactory.Setup(cf => cf.CreateClient(It.IsAny<string>())).Returns(httpClient);
        }

        private FunTranslationClient CreateFunTranslationClient()
        {
            return new FunTranslationClient(_mockHttpClientFactory.Object);
        }

        [Test]
        public async Task TranslateToShakespeareAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            _clientHandlerMock.Protected()
                            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(_content) })
                            .Verifiable();

            var funTranslationClient = this.CreateFunTranslationClient();
            string description = "translateMe";

            // Act
            var result = await funTranslationClient.TranslateToShakespeareAsync(
                description);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, "translated");
            _clientHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task TranslateToYodaAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            _clientHandlerMock.Protected()
                            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(_content) })
                            .Verifiable();

            var funTranslationClient = this.CreateFunTranslationClient();
            string description = "translateMe";

            // Act
            var result = await funTranslationClient.TranslateToYodaAsync(description);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, "translated");
            _clientHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }
    }
}
