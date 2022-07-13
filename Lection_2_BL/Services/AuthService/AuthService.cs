using Lection_2_BL.Auth;
using Lection_2_BL.DTOs;
using Lection_2_BL.Services.HashService;
using Lection_2_DAL;
using Lection_2_DAL.Entities;
using System;
using System.Threading.Tasks;

namespace Lection_2_BL.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _genericClientRepository;
        private readonly IGenericRepository<Role> _genericRoleRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IHashService _hashService;

        public AuthService(
            IGenericRepository<User> genericClientRepository,
            IGenericRepository<Role> genericRoleRepository,
            ITokenGenerator tokenGenerator,
            IHashService hashService)
        {
            _genericClientRepository = genericClientRepository;
            _genericRoleRepository = genericRoleRepository;
            _tokenGenerator = tokenGenerator;
            _hashService = hashService;
        }

        public async Task<string> SignIn(string login, string password)
        {
            var user = await _genericClientRepository.GetByPredicate(
                x => x.Email == login && x.Password == _hashService.HashString(password));

            if (user == null)
            {
                throw new ArgumentException();
            }

            var role = user.RoleId.HasValue ? (await _genericRoleRepository.GetById(user.RoleId.Value)).Name : Roles.Reader;

            return _tokenGenerator.GenerateToken(user.Email, role);
        }

        public async Task<Guid> SignUp(UserDto user)
        {
            user.Password = _hashService.HashString(user.Password);

            return await _genericClientRepository.Add(Map(user));
        }

        private User Map(UserDto userDto)
        {
            return new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Password = userDto.Password,
                Email = userDto.Email,
                BirthDate = userDto.BirthDate
            };
        }
    }
}
