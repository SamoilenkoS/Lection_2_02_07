using System;
using System.Collections.Generic;

namespace Lection_2_DAL
{
    public interface IBooksRepository
    {
        IEnumerable<Book> GetAll();
        Book GetById(Guid id);
        bool DeleteById(Guid id);
        bool Update(Book book);
        Guid Add(Book book);
    }
}
