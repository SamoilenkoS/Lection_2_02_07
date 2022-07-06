using Lection_2_DAL;
using Lection_2_DAL.Entities;

namespace Lection_2_BL
{
    public interface IClientService
    {
        bool RentABook(Book book, Client client);
        bool ReturnABook(Book book, Client client);
    }
}
