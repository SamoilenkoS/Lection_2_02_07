using Lection_2_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lection_2_BL
{
    public interface IBooksService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(Guid id);
        Task<bool> DeleteBookById(Guid id);
        Task<bool> UpdateBook(Book book);
        Task<Guid> AddBook(Book book);
    }
}
