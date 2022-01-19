using Newtonsoft.Json;
using Podedex.Models;
using Podedex.Models.Pokemon;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Podedex.Services.Clients
{
    public class PokemonClient : IPokemonClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PokemonClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Return Pokemon
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<PokemonVM> GetPokemonAsync(string name)
        {
            var client = _httpClientFactory.CreateClient(PokemonConstents.Pokeapi);

            PokemonResponse pokemonResponse = null;
            
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri($"{client.BaseAddress}/v2/pokemon-species/{name}"));

            client.DefaultRequestHeaders
                              .Accept
                              .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(httpRequestMessage);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                pokemonResponse = JsonConvert.DeserializeObject<PokemonResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Formatting = Formatting.Indented });
                return MapToPokemon(pokemonResponse);
            }
            else
                return null;
        }

        /// <summary>
        /// Map PokemonRespone to PokemonVM
        /// </summary>
        /// <param name="pokemonResponse"></param>
        /// <returns></returns>
        private PokemonVM MapToPokemon(PokemonResponse pokemonResponse)
        {
            return new PokemonVM
            {
                Description = pokemonResponse.flavor_text_entries?.FirstOrDefault(x => x.language?.name?.ToLower() == PokemonConstents.EnglishLanguage)?.flavor_text,
                Habitat = pokemonResponse.habitat?.name,
                IsLegendary = pokemonResponse.is_legendary,
                Name = pokemonResponse.name
            };
        }
    }
}
