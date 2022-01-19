using Podedex.Models.Pokemon;
using System.Threading.Tasks;

namespace Podedex.Services.PodedexServices
{
    public interface IPodedexService
    {
        Task<PokemonVM> GetPokemonAsync(string name);
        Task<PokemonVM> TranslateAsync(string name);
    }
}
