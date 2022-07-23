using Lection_2_DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lection_2_BL.Services
{
    public interface ILibraryService
    {
        Task<IEnumerable<Library>> GetNearestLibraries(
            Point userLocation,
            int count = 10);
    }
}
