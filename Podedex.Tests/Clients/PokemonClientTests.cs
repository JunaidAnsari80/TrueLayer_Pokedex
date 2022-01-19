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
    public class PokemonClientTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<DelegatingHandler> _clientHandlerMock;
        private string _content = "{\"flavor_text_entries\": [{\"flavor_text\": \"test\",\"language\": { \"name\": \"en\",  \"url\": \"www.google.com\"},\"version\": { \"name\": \"2\",  \"url\": \"www.google.com\"} }],\"habitat\": {\"name\": \"cave\", \"url\": \"www.google.com\"},\"is_legendary\": false, \"name\": \"test\"}";
        
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

        private PokemonClient CreatePokemonClient()
        {
            return new PokemonClient(_mockHttpClientFactory.Object);
        }

        [Test]
        public async Task GetPokemonAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            _clientHandlerMock.Protected()
                                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                                   .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(_content) })
                                   .Verifiable();
            var pokemonClient = CreatePokemonClient();
            string name = "Test";

            // Act
            var result = await pokemonClient.GetPokemonAsync(name);

            // Assert
            Assert.IsNotNull(result);
            _clientHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetPokemonAsync_StateUnderTest_UnexpectedBehavior()
        {
            // Arrange
            _clientHandlerMock.Protected()
                                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                                   .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent("") })
                                   .Verifiable();
            var pokemonClient = CreatePokemonClient();
            string name = "Test";

            // Act
            var result = await pokemonClient.GetPokemonAsync(name);

            // Assert
            Assert.IsNull(result);
            _clientHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }
    }
}
