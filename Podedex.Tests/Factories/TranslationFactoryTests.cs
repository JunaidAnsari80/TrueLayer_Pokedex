using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Podedex.Models.Pokemon;
using Podedex.Services.Clients;
using Podedex.Services.Factories;
using System;
using System.Threading.Tasks;

namespace Podedex.Tests.Factories
{
    [TestFixture]
    public class TranslationFactoryTests
    {
        private Mock<IFunTranslationClient> _mockFunTranslationClient;
        private Mock<ILogger<TranslationFactory>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockFunTranslationClient = new Mock<IFunTranslationClient>();
            _mockLogger = new Mock<ILogger<TranslationFactory>>();
        }

        private TranslationFactory CreateFactory()
        {
            return new TranslationFactory(
                _mockFunTranslationClient.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task TranslateIt_Shakespeare_StateUnderTest_When_ExpectedBehavior()
        {
            // Arrange
            var factory = this.CreateFactory();
            PokemonVM pokemon = new PokemonVM
            {
                Description = "TranslateMe",
                Habitat = "Home"               
            };
            var translated = "Shakespeare Translated";
            _mockFunTranslationClient.Setup(x => x.TranslateToShakespeareAsync(It.IsAny<string>())).ReturnsAsync(translated);

            // Act
            var result = await factory.TranslateIt(
                pokemon);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, translated);
            _mockFunTranslationClient.Verify(x => x.TranslateToShakespeareAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task TranslateIt_Shakespeare_StateUnderTest_When_UnexpectedBehavior()
        {
            // Arrange
            var factory = this.CreateFactory();
            PokemonVM pokemon = new PokemonVM
            {
                Description = "TranslateMe",
                Habitat = "Home"
            };
            
            _mockFunTranslationClient.Setup(x => x.TranslateToShakespeareAsync(It.IsAny<string>())).ThrowsAsync(new Exception() { });

            // Act
            var result = await factory.TranslateIt(
                pokemon);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, pokemon.Description);
            _mockFunTranslationClient.Verify(x => x.TranslateToShakespeareAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task TranslateIt_Yoda_StateUnderTest_When_ExpectedBehavior()
        {
            // Arrange
            var factory = this.CreateFactory();
            PokemonVM pokemon = new PokemonVM
            {
                Description = "TranslateMe",
                Habitat = "cave"
            };
            var translated = "Yoda Translated";
            _mockFunTranslationClient.Setup(x => x.TranslateToYodaAsync(It.IsAny<string>())).ReturnsAsync(translated);

            // Act
            var result = await factory.TranslateIt(
                pokemon);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, translated);
            _mockFunTranslationClient.Verify(x => x.TranslateToYodaAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task TranslateIt_Yoda_StateUnderTest_When_UnexpectedBehavior()
        {
            // Arrange
            var factory = this.CreateFactory();
            PokemonVM pokemon = new PokemonVM
            {
                Description = "TranslateMe",
                Habitat = "cave"
            };

            _mockFunTranslationClient.Setup(x => x.TranslateToYodaAsync(It.IsAny<string>())).ThrowsAsync(new Exception() { });

            // Act
            var result = await factory.TranslateIt(
                pokemon);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, pokemon.Description);
            _mockFunTranslationClient.Verify(x => x.TranslateToYodaAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
