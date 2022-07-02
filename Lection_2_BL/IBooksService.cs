using Lection_2_DAL;
using System;
using System.Collections.Generic;

namespace Lection_2_BL
{
    public interface IBooksService
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(Guid id);
        bool DeleteBookById(Guid id);
        bool UpdateBook(Book book);
        Guid AddBook(Book book);
    }
}
