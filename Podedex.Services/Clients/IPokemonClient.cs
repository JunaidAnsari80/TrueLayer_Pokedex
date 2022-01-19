using Podedex.Models.Pokemon;
using System.Threading.Tasks;

namespace Podedex.Services.Clients
{
    public interface IPokemonClient
    {
        Task<PokemonVM> GetPokemonAsync(string name);
    }
}
