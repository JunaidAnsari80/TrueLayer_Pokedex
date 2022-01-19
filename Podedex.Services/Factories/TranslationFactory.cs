using Microsoft.Extensions.Logging;
using Podedex.Models.Pokemon;
using Podedex.Services.Clients;
using System;
using System.Threading.Tasks;

namespace Podedex.Services.Factories
{
    public class TranslationFactory : ITranslationFactory
    {
        private readonly IFunTranslationClient _funTranslationClient;
        private ILogger<TranslationFactory> _logger;
        public TranslationFactory(IFunTranslationClient funTranslationClient , ILogger<TranslationFactory> logger)
        {
            _funTranslationClient = funTranslationClient;
            _logger = logger;
        }

        /// <summary>
        /// Factory method to decide what to translate
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public async Task<string> TranslateIt(PokemonVM pokemon)
        {
            try
            {
                if (pokemon.Habitat == "cave" || pokemon.IsLegendary)
                {
                    pokemon.Description = await _funTranslationClient.TranslateToYodaAsync(pokemon.Description);
                }
                else
                {
                    pokemon.Description = await _funTranslationClient.TranslateToShakespeareAsync(pokemon.Description);
                }
            }
            catch(Exception exp)
            {
                _logger.LogError(exp, "Exception has occured while translating pokemon description");
            }

            return pokemon.Description;
        }
    }
}
