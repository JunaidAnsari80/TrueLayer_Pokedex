using System.Threading.Tasks;

namespace Podedex.Services.Clients
{
    public interface IFunTranslationClient
    {
        Task<string> TranslateToShakespeareAsync(string description);

        Task<string> TranslateToYodaAsync(string description);
    }
}
