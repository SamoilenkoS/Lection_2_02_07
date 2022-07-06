using Lection_2_DAL;
using Lection_2_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lection_2_BL
{
    public class BooksService : IBooksService
    {
        private readonly IGenericRepository<Book> _booksRepository;

        public BooksService(IGenericRepository<Book> booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public async Task<Guid> AddBook(Book book)
        {
            return await _booksRepository.Add(book);
        }

        public async Task<bool> DeleteBookById(Guid id)
        {
            return await _booksRepository.DeleteById(id);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _booksRepository.GetAll();
        }

        public async Task<Book> GetBookById(Guid id)
        {
            return await _booksRepository.GetById(id);
        }

        public async Task<bool> UpdateBook(Book book)
        {
            return await _booksRepository.Update(book);
        }
    }
}
