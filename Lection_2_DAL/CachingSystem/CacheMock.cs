using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_DAL.CachingSystem
{
    public class CacheMock : ICacheRepository
    {
        public Task<string> GetAsync(string key)
        {
            return Task.FromResult(string.Empty);
        }

        public Task SaveAsync(string key, string value)
        {
            return Task.CompletedTask;
        }
    }
}
