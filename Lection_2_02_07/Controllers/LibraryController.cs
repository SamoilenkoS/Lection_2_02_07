using Lection_2_BL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lection_2_02_07.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLibraries(float latitude, float longitude)
        {
            return Ok(
                await _libraryService.GetNearestLibraries(
                    new Lection_2_DAL.Entities.Point
                    {
                        Latitude = latitude,
                        Longitude = longitude
                    }));
        }
    }
}
