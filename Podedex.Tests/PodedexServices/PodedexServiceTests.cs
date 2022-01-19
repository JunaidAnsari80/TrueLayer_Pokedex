using Moq;
using NUnit.Framework;
using Podedex.Models.Pokemon;
using Podedex.Services.Clients;
using Podedex.Services.Factories;
using Podedex.Services.PodedexServices;
using System.Threading.Tasks;

namespace Podedex.Tests.PodedexServices
{
    [TestFixture]
    public class PodedexServiceTests
    {
        private Mock<IPokemonClient> _mockPokemonClient;
        private Mock<ITranslationFactory> _mockTranslationFactory;
        private PokemonVM _pokemon;
        private string _pokemonName = "test";

        [SetUp]
        public void SetUp()
        {
            _mockPokemonClient = new Mock<IPokemonClient>();
            _mockTranslationFactory = new Mock<ITranslationFactory>();
            _pokemon = new PokemonVM
            {
                Description = "test",
                Habitat = "test",
                IsLegendary = true,
                Name = "test"
            };            
        }

        private PodedexService CreateService()
        {
            return new PodedexService(
                _mockPokemonClient.Object,
                _mockTranslationFactory.Object);
        }

        [Test]
        public async Task GetPokemonAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();           
            _mockPokemonClient.Setup(x => x.GetPokemonAsync(It.IsAny<string>())).ReturnsAsync(_pokemon);

            // Act
            var result = await service.GetPokemonAsync(_pokemonName);

            // Assert
            Assert.IsNotNull(result);            
        }

        [Test]
        public async Task GetPokemonAsync_StateUnderTest_UnexpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            _mockPokemonClient.Setup(x => x.GetPokemonAsync(It.IsAny<string>())).ReturnsAsync(default(PokemonVM));

            // Act
            var result = await service.GetPokemonAsync(_pokemonName);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task TranslateAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string translation = "translated";
            var service = this.CreateService();
            _mockPokemonClient.Setup(x => x.GetPokemonAsync(It.IsAny<string>())).ReturnsAsync(_pokemon);
            _mockTranslationFactory.Setup(x => x.TranslateIt(It.IsAny<PokemonVM>())).ReturnsAsync(translation);
            
            // Act
            var result = await service.TranslateAsync(_pokemonName);

            // Assert
            Assert.IsNotNull(result);
            Assert.True(translation == result.Description);
        }

        [Test]
        public async Task TranslateAsync_StateUnderTest_UnExpectedBehavior()
        {
            // Arrange         
            var service = this.CreateService();
            _mockPokemonClient.Setup(x => x.GetPokemonAsync(It.IsAny<string>())).ReturnsAsync(_pokemon);
            _mockTranslationFactory.Setup(x => x.TranslateIt(It.IsAny<PokemonVM>())).ReturnsAsync(default(string));

            // Act
            var result = await service.TranslateAsync(_pokemonName);

            // Assert
            Assert.IsNull(result.Description);           
        }       
    }
}
