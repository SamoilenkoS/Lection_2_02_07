using Lection_2_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lection_2_DAL
{
    public class BooksRepository : IBooksRepository
    {
        private readonly EFCoreDbContext _dbContext;

        public BooksRepository(EFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(Book book, IEnumerable<BookRevision> bookRevisions)> GetFullInfo(Guid id)
        {
            var result = await _dbContext.BookRevisions.Include(x => x.Book).Where(x => x.BookId == id).ToListAsync();

            var book = result.FirstOrDefault()?.Book;

            return (book, result);
        }
    }
}
