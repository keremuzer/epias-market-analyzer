using System.Threading.Tasks;

namespace EpiasMarketAnalyzer.Services
{
    public interface IAuthService
    {
        Task<string> GetTgtAsync();
    }
}