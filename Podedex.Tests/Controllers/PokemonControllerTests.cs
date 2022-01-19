using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Podedex.Models.Pokemon;
using Podedex.Services.PodedexServices;
using Pokedex.API.Controllers;
using System.Threading.Tasks;

namespace Podedex.Tests.Controllers
{
    [TestFixture]
    public class PokemonControllerTests
    {
        private Mock<IPodedexService> _mockPodedexService;
        private PokemonVM _pokemon;

        [SetUp]
        public void SetUp()
        {
            _mockPodedexService = new Mock<IPodedexService>();
            _pokemon = new PokemonVM
            {
                Description = "test",
                Habitat = "test",
                IsLegendary = true,
                Name = "test"
            };
        }

        private PokemonController CreatePokemonController()
        {
            return new PokemonController(_mockPodedexService.Object);
        }

        [Test]
        public async Task Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var pokemonController = CreatePokemonController();

            _mockPodedexService.Setup(x => x.GetPokemonAsync(It.IsAny<string>())).ReturnsAsync(_pokemon);

            // Act
            var result = await pokemonController.Get("pokemon");

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result is OkObjectResult);
            _mockPodedexService.Verify(x => x.GetPokemonAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Get_StateUnderTest_UnexpectedBehavior()
        {
            // Arrange
            var pokemonController = CreatePokemonController();

            _mockPodedexService.Setup(x => x.GetPokemonAsync(It.IsAny<string>())).ReturnsAsync(default(PokemonVM));

            // Act
            var result = await pokemonController.Get("pokemon");

            // Assert           
            Assert.That(result is NotFoundResult);
            _mockPodedexService.Verify(x => x.GetPokemonAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetFunTranslation_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var pokemonController = CreatePokemonController();

            _mockPodedexService.Setup(x => x.TranslateAsync(It.IsAny<string>())).ReturnsAsync(_pokemon);

            // Act
            var result = await pokemonController.GetFunTranslation("pokemon");

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result is OkObjectResult);
            _mockPodedexService.Verify(x => x.TranslateAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetFunTranslation_StateUnderTest_UnexpectedBehavior()
        {
            // Arrange
            var pokemonController = CreatePokemonController();

            _mockPodedexService.Setup(x => x.TranslateAsync(It.IsAny<string>())).ReturnsAsync(default(PokemonVM));

            // Act
            var result = await pokemonController.GetFunTranslation("pokemon");

            // Assert           
            Assert.That(result is NotFoundResult);
            _mockPodedexService.Verify(x => x.TranslateAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
