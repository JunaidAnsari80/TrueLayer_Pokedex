using Podedex.Models.Pokemon;
using Podedex.Services.Clients;
using Podedex.Services.Factories;
using System.Threading.Tasks;

namespace Podedex.Services.PodedexServices
{
    public class PodedexService : IPodedexService
    {
        private readonly IPokemonClient _pokemonClient;
        private readonly ITranslationFactory _translationFactory;

        public PodedexService(IPokemonClient pokemonClient, ITranslationFactory translationFactory)
        {
            _pokemonClient = pokemonClient;
            _translationFactory = translationFactory;
        }


        /// <summary>
        /// Get Pokemon from downstream against name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<PokemonVM> GetPokemonAsync(string name)
        {
            return GetPokemonInternalAsync(name);
        }       

        /// <summary>
        /// Get Pokemon from downstream against name and perform transaction
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<PokemonVM> TranslateAsync(string name)
        {
            var pokemon = await GetPokemonInternalAsync(name);

            if (pokemon == null)
            {
                return null;
            }

            pokemon.Description = await _translationFactory.TranslateIt(pokemon);
            
            return pokemon;
        }

        private Task<PokemonVM> GetPokemonInternalAsync(string name)
        {
            return _pokemonClient.GetPokemonAsync(name);
        }
    }
}
