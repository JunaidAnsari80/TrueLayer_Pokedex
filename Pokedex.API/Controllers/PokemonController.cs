using Microsoft.AspNetCore.Mvc;
using Podedex.Models.Pokemon;
using Podedex.Services.PodedexServices;
using System.Net;
using System.Threading.Tasks;

namespace Pokedex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPodedexService _podedexService;

        public PokemonController(IPodedexService podedexService)
        {
            _podedexService = podedexService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("{name}")]
        [HttpGet]
        [ProducesResponseType(typeof(PokemonVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute]string name)
        {
            var result = await _podedexService.GetPokemonAsync(name);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("translated/{name}")]
        [HttpGet]
        [ProducesResponseType(typeof(PokemonVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetFunTranslation(string name)
        {
            var result = await _podedexService.TranslateAsync(name);

            if (result == null)
                return NotFound();
           
            return Ok(result);
        }
    }
}
