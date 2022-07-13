using Lection_2_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
