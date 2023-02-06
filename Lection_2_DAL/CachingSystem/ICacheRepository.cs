using System.Threading.Tasks;

namespace Lection_2_DAL.CachingSystem
{
    public interface ICacheRepository
    {
        Task SaveAsync(string key, string value);
        Task<string> GetAsync(string key);
    }
}
