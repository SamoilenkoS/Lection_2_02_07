using Lection_2_DAL;
using System;
using System.Collections.Generic;

namespace Lection_2_BL
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;

        public BooksService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public Guid AddBook(Book book)
        {
            ValidateBookState(book);

            return _booksRepository.Add(book);
        }

        public bool DeleteBookById(Guid id)
        {
            return _booksRepository.DeleteById(id);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _booksRepository.GetAll();
        }

        public Book GetBookById(Guid id)
        {
            return _booksRepository.GetById(id);
        }

        public bool UpdateBook(Book book)
        {
            ValidateBookState(book);

            return _booksRepository.Update(book);
        }

        private static void ValidateBookState(Book book)
        {
            if (book.PagesCount < 10 || book.PagesCount > 2_000)
            {
                throw new ArgumentException("Invalid pages count!");
            }
        }
    }
}
