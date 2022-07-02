using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lection_2_DAL
{
    public class BooksRepository : IBooksRepository
    {
        private readonly EFCoreDbContext _dbContext;

        public BooksRepository(EFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Add(Book book)
        {
            book.Id = Guid.NewGuid();
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

            return book.Id;
        }

        public bool DeleteById(Guid id)
        {
            var book = new Book { Id = id };
            _dbContext.Entry(book).State = EntityState.Deleted;

            return _dbContext.SaveChanges() != 0;
        }

        public IEnumerable<Book> GetAll()
        {
            return _dbContext.Books.ToList();
        }

        public Book GetById(Guid id)
        {
            return _dbContext.Books.Where(x => x.Id == id).FirstOrDefault();
        }

        public bool Update(Book book)
        {
            _dbContext.Entry(book).State = EntityState.Modified;

            return _dbContext.SaveChanges() != 0;
        }
    }
}
