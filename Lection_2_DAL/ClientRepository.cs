using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_DAL
{
    public class ClientRepository : IClientRepository
    {
        private readonly EFCoreDbContext _dbContext;

        public ClientRepository(EFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Add(Client client)
        {
            client.Id = Guid.NewGuid();
            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();

            return client.Id;
        }

        public bool DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetAll()
        {
            return _dbContext.Clients.ToList();
        }

        public Client GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
