using Podedex.Models.Pokemon;
using System.Threading.Tasks;

namespace Podedex.Services.Factories
{
    public interface ITranslationFactory
    {
        Task<string> TranslateIt(PokemonVM pokemon);
    }
}
