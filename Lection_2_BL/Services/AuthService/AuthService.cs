using Lection_2_BL.Auth;
using Lection_2_DAL;
using Lection_2_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_BL.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _genericClientRepository;
        private readonly IGenericRepository<Role> _genericRoleRepository;

        public AuthService(
            IGenericRepository<User> genericClientRepository,
            IGenericRepository<Role> genericRoleRepository)
        {
            _genericClientRepository = genericClientRepository;
            _genericRoleRepository = genericRoleRepository;
        }

        public async Task<string> SignIn(string login, string password)
        {
            var user = await _genericClientRepository.GetByPredicate(x => x.Email == login && x.Password == password);

            if (user == null)
            {
                throw new ArgumentException();
            }

            var role = user.RoleId.HasValue ? (await _genericRoleRepository.GetById(user.RoleId.Value)).Name : Roles.Reader;

            return TokenGenerator.GenerateToken(user.Email, role);
        }
    }
}
