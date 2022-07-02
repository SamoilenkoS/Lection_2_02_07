using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_DAL
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAll();
        Client GetById(Guid id);
        bool DeleteById(Guid id);
        bool Update(Client client);
        Guid Add(Client client);
    }
}
