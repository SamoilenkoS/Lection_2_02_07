using Lection_2_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_BL
{
    public interface IClientService
    {
        bool RentABook(Book book, Client client);
        bool ReturnABook(Book book, Client client);
    }
}
