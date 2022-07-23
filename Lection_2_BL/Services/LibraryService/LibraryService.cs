using Lection_2_DAL;
using Lection_2_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lection_2_BL.Services.LibraryService
{
    public class LibraryService : ILibraryService
    {
        private readonly IGenericRepository<Library> _genericLibraryRepository;

        public LibraryService(IGenericRepository<Library> genericLibraryRepository)
        {
            _genericLibraryRepository = genericLibraryRepository;
        }

        public async Task<IEnumerable<Library>> GetNearestLibraries(
            Point userLocation, int count = 10)
        {
            return await _genericLibraryRepository.GetAllByPredicate(x =>
            Math.Sqrt(
                       Math.Pow(10 - x.Location.Latitude, 2)
                       +
                       Math.Pow(5 - x.Location.Longitude, 2)) < 10);
        }
    }
}
