using Lection_2_BL.Services.BooksService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_BL.Jobs
{
    public class CountMonitorJob
    {
        private readonly IBooksService _booksService;
        private readonly ILogger<CountMonitorJob> _logger;

        public CountMonitorJob(
            IBooksService booksService,
            ILogger<CountMonitorJob> logger)
        {
            _booksService = booksService;
            _logger = logger;
        }

        public async Task StartJob()
        {
            var count = (await _booksService.GetAllBooks()).Count();

            _logger.LogInformation("Count:{0}", count);
        }
    }
}
