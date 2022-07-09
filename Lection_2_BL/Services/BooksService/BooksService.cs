using Lection_2_BL.DTOs;
using Lection_2_DAL;
using Lection_2_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lection_2_BL.Services.BooksService
{
    public class BooksService : IBooksService
    {
        private readonly IGenericRepository<Book> _genericBooksRepository;
        private readonly IBooksRepository _booksRepository;

        public BooksService(
            IGenericRepository<Book> genericBooksRepository,
            IBooksRepository booksRepository)
        {
            _genericBooksRepository = genericBooksRepository;
            _booksRepository = booksRepository;
        }

        public async Task<Guid> AddBook(Book book)
        {
            return await _genericBooksRepository.Add(book);
        }

        public async Task<bool> DeleteBookById(Guid id)
        {
            return await _genericBooksRepository.DeleteById(id);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _genericBooksRepository.GetAll();
        }

        public async Task<Book> GetBookById(Guid id)
        {
            return await _genericBooksRepository.GetById(id);
        }

        public async Task<bool> UpdateBook(Book book)
        {
            return await _genericBooksRepository.Update(book);
        }

        public async Task<BookDto> GetBookFullInfo(Guid id)
        {
            var result = await _booksRepository.GetFullInfo(id);

            return MapTupleToBookDto(result);
        }

        private BookDto MapTupleToBookDto((Book book, IEnumerable<BookRevision> bookRevisions) result)
        {
            return new BookDto
            {
                Author = result.book?.Author,
                BookId = result.book.Id,
                Title = result.book.Title,
                BookRevisions = MapRevisions(result.bookRevisions)
            };
        }

        private IEnumerable<BookRevisionDto> MapRevisions(IEnumerable<BookRevision> bookRevisions)
        {
            return bookRevisions.Select(x => new BookRevisionDto
            {
                Price = x.Price,
                PagesCount = x.PagesCount,
                YearOfPublishing = x.YearOfPublishing
            });
        }
    }
}
