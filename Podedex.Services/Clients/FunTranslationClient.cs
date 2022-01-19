using Newtonsoft.Json;
using Podedex.Models;
using Podedex.Models.FunTranslation;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Podedex.Services.Clients
{
    public class FunTranslationClient : IFunTranslationClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FunTranslationClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Translate to Shakespare. If anything happen to translation and it return null than send original description back. Excluding Exception
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Task<string> TranslateToShakespeareAsync(string description)
        {
            return Translate("shakespeare.json", description);
        }

        /// <summary>
        /// Translate to Yoda. If anything happen to translation and it return null than send original description back. Excluding Exception
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Task<string> TranslateToYodaAsync(string description)
        {
            return Translate("yoda.json", description);
        }

        /// <summary>
        /// Send translation call to 3rd party and keeping the escape characters
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        private async Task<string> Translate(string source, string description)
        {         
            string resource = $"translate/{source}?text={description}";

            var client = _httpClientFactory.CreateClient(PokemonConstents.Funtranslations);

            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri($"{client.BaseAddress}{resource}")
               );

            client.DefaultRequestHeaders
                             .Accept
                             .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(httpRequestMessage);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<TranslationResponse>(await response.Content.ReadAsStringAsync());
                return Unescape(result.contents.translated);
            }
            else
                return description;
        }      
       
        /// <summary>
        /// Manage escape
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        private string Unescape(string translation)
        {
            return Regex.Unescape(translation);           
        }
    }
}
