using Lection_2_BL.Auth;
using Lection_2_BL.DTOs;
using Lection_2_BL.Services.EncryptionService;
using Lection_2_BL.Services.HashService;
using Lection_2_BL.Services.SMTPService;
using Lection_2_DAL;
using Lection_2_DAL.CachingSystem;
using Lection_2_DAL.Entities;
using System;
using System.Threading.Tasks;

namespace Lection_2_BL.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly ISendingBlueSmtpService _sendingBlueSmtpService;
        private readonly IGenericRepository<User> _genericClientRepository;
        private readonly IGenericRepository<Role> _genericRoleRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IHashService _hashService;
        private readonly ICacheRepository _cacheRepository;

        public AuthService(
            IEncryptionService encryptionService,
            ISendingBlueSmtpService sendingBlueSmtpService,
            IGenericRepository<User> genericClientRepository,
            IGenericRepository<Role> genericRoleRepository,
            ICacheRepository cacheRepository,
            ITokenGenerator tokenGenerator,
            IHashService hashService)
        {
            _encryptionService = encryptionService;
            _sendingBlueSmtpService = sendingBlueSmtpService;
            _genericClientRepository = genericClientRepository;
            _genericRoleRepository = genericRoleRepository;
            _cacheRepository = cacheRepository;
            _tokenGenerator = tokenGenerator;
            _hashService = hashService;
        }

        public async Task<string> SignIn(string login, string password)
        {
            var hashed = _hashService.HashString(password);
            var user = await _genericClientRepository.GetByPredicate(
                x => x.Email == login && x.Password == hashed);

            if (user == null)
            {
                throw new ArgumentException();
            }

            var role = user.RoleId.HasValue ? (await GetRole(user.RoleId.Value)) : Roles.Reader;

            return _tokenGenerator.GenerateToken(user.Email, role);
        }

        private async Task<string> GetRole(Guid roleId)
        {
            var cachedRole = await _cacheRepository.GetAsync(roleId.ToString());
            if (string.IsNullOrEmpty(cachedRole))
            {
                cachedRole = (await _genericRoleRepository.GetById(roleId)).Name;
                await _cacheRepository.SaveAsync(roleId.ToString(), cachedRole);
            }

            return cachedRole;
        }

        public async Task<Guid> SignUp(UserDto user)
        {
            var hashed = _hashService.HashString(user.Password);

            var response = await _genericClientRepository.Add(Map(user, hashed));

            await _sendingBlueSmtpService.SendMail(
                new MailInfo
                {
                    ClientName = $"{user.FirstName} {user.LastName}",
                    Email = user.Email,
                    Subject = "Email confirmation",
                    Body = "https://localhost:5001/users/confirm?email="
                    +GenerateConfirmationString(user.Email)
                });

            return response;
        }

        private string GenerateConfirmationString(string email)
        {
            return _encryptionService.EncryptString(email);
        }

        public async Task<bool> ConfirmUserMail(string encryptedEmail)
        {
            encryptedEmail = encryptedEmail.Replace(' ', '+');
            var userEmail = _encryptionService.DecryptString(encryptedEmail);

            var user = await _genericClientRepository.GetByPredicate(
                x => x.Email == userEmail);

            if(user != null)
            {
                user.IsConfirmed = true;
                await _genericClientRepository.Update(user);
            }

            return user != null;
        }

        private User Map(UserDto userDto, string hashed)
        {
            return new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Password = hashed,
                Email = userDto.Email,
                BirthDate = userDto.BirthDate
            };
        }
    }
}
