using AutoFixture;
using Lection_2_BL.Auth;
using Lection_2_BL.DTOs;
using Lection_2_BL.Services.AuthService;
using Lection_2_BL.Services.EncryptionService;
using Lection_2_BL.Services.HashService;
using Lection_2_BL.Services.SMTPService;
using Lection_2_DAL;
using Lection_2_DAL.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Lection_2_BL.Tests
{
    public class AuthServiceTests
    {
        private Mock<IEncryptionService> _encryptionServiceMock;
        private Mock<ISendingBlueSmtpService> _sendingBlueSmtpServiceMock;
        private Mock<IGenericRepository<User>> _genericUserRepositoryMock;
        private Mock<IGenericRepository<Role>> _genericRoleRepositoryMock;
        private Mock<ITokenGenerator> _tokenGeneratorMock;
        private Mock<IHashService> _hashService;
        private AuthService _authService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _sendingBlueSmtpServiceMock = new Mock<ISendingBlueSmtpService>();
            _genericUserRepositoryMock = new Mock<IGenericRepository<User>>();
            _genericRoleRepositoryMock = new Mock<IGenericRepository<Role>>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _hashService = new Mock<IHashService>();
            _authService = new AuthService(
                _encryptionServiceMock.Object,
                _sendingBlueSmtpServiceMock.Object,
                _genericUserRepositoryMock.Object,
                _genericRoleRepositoryMock.Object,
                _tokenGeneratorMock.Object,
                _hashService.Object);

            _fixture = new Fixture();
        }

        [Test]
        public async Task SignIn_WhenUserWithoutRole_ShouldLoginUserWithReader()
        {
            var login = _fixture.Create<string>();
            var password = _fixture.Create<string>();
            var hashPassword = _fixture.Create<string>();
            var expectedToken = _fixture.Create<string>();
            var userInDb = _fixture.Create<User>();
            userInDb.Email = login;
            userInDb.RoleId = null;
            userInDb.Role = null;

            _hashService
                .Setup(x => x.HashString(password))
                .Returns(hashPassword)
                .Verifiable();
            _genericUserRepositoryMock
                .Setup(
                    x =>
                    x.GetByPredicate(
                        y => y.Email == login && y.Password == hashPassword))
                .ReturnsAsync(userInDb)
                .Verifiable();
            _tokenGeneratorMock
                .Setup(x => x.GenerateToken(login, Roles.Reader))
                .Returns(expectedToken)
                .Verifiable();

            var actualToken = await _authService.SignIn(login, password);

            _hashService.Verify();
            _genericUserRepositoryMock.Verify();
            _tokenGeneratorMock.Verify();
            _genericRoleRepositoryMock.Verify(
                x => x.GetById(It.IsAny<Guid>()), Times.Never);
            Assert.AreEqual(expectedToken, actualToken);
        }

        [Test]
        public async Task SignIn_WhenUserWithRole_ShouldLoginUserWithRoleFromDb()
        {
            var login = _fixture.Create<string>();
            var password = _fixture.Create<string>();
            var hashPassword = _fixture.Create<string>();
            var expectedToken = _fixture.Create<string>();
            var userInDb = _fixture.Create<User>();
            userInDb.Role = null;
            var role = _fixture.Create<Role>();
            userInDb.Email = login;

            _hashService
                .Setup(x => x.HashString(password))
                .Returns(hashPassword)
                .Verifiable();
            _genericUserRepositoryMock
                .Setup(
                    x =>
                    x.GetByPredicate(
                        y => y.Email == login && y.Password == hashPassword))
                .ReturnsAsync(userInDb)
                .Verifiable();
            _genericRoleRepositoryMock
                .Setup(x => x.GetById(userInDb.RoleId.Value))
                .ReturnsAsync(role);
            _tokenGeneratorMock
                .Setup(x => x.GenerateToken(login, role.Name))
                .Returns(expectedToken)
                .Verifiable();

            var actualToken = await _authService.SignIn(login, password);

            _hashService.Verify();
            _genericUserRepositoryMock.Verify();
            _tokenGeneratorMock.Verify();
            _genericRoleRepositoryMock.Verify();
            Assert.AreEqual(expectedToken, actualToken);
        }

        [Test]
        public async Task SignIn_WhenNoUserInDb_ShouldThrowArgumentException()
        {
            var login = _fixture.Create<string>();
            var password = _fixture.Create<string>();
            var hashPassword = _fixture.Create<string>();

            _hashService
                .Setup(x => x.HashString(password))
                .Returns(hashPassword)
                .Verifiable();
            _genericUserRepositoryMock
                .Setup(
                    x =>
                    x.GetByPredicate(
                        y => y.Email == login && y.Password == hashPassword))
                .Verifiable();

            Assert.ThrowsAsync<ArgumentException>(
                async () => await _authService.SignIn(login, password));

            _hashService.Verify();
            _genericUserRepositoryMock.Verify();
            _tokenGeneratorMock
                .Verify(x => x.GenerateToken(
                    It.IsAny<string>(),
                    It.IsAny<string>()), Times.Never);
            _genericRoleRepositoryMock
                .Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task SignUp_WhenCalled_ShouldRegisterUser()
        {
            var userDto = _fixture.Create<UserDto>();
            var hashPassword = _fixture.Create<string>();
            var expectedGuid = Guid.NewGuid();
            var encryptedMail = _fixture.Create<string>();
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Password = userDto.Password,
                Email = userDto.Email,
                BirthDate = userDto.BirthDate
            };
            var mailInfo = new MailInfo
            {
                ClientName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Subject = "Email confirmation",
                Body = "https://localhost:5001/users/confirm?email="
                    + encryptedMail
            };

            _hashService
                .Setup(x => x.HashString(userDto.Password))
                .Returns(hashPassword)
                .Verifiable();
            _genericUserRepositoryMock
                .Setup(x => x.Add(
                    It.Is<User>(y =>
                        y.Id == user.Id
                        && y.FirstName == user.FirstName
                        && y.LastName == user.LastName
                        && y.Email == user.Email)))
                .ReturnsAsync(expectedGuid)
                .Verifiable();
            _encryptionServiceMock
                .Setup(x =>
                x.EncryptString(userDto.Email))
                .Returns(encryptedMail)
                .Verifiable();
            _sendingBlueSmtpServiceMock
                .Setup(x => x.SendMail(
                    It.Is<MailInfo>(x =>
                        x.Email == mailInfo.Email
                        && x.ClientName == mailInfo.ClientName
                        && x.Body == mailInfo.Body
                        && x.Subject == mailInfo.Subject)))
                .Verifiable();

            var actualGuid = await _authService.SignUp(userDto);

            _hashService.Verify();
            _genericUserRepositoryMock.Verify();
            _encryptionServiceMock.Verify();
            _sendingBlueSmtpServiceMock.Verify();
            Assert.AreEqual(expectedGuid, actualGuid);
        }

        [Test]
        public async Task ConfirmUserMail_WhenUserExist_ShouldConfirmEmail()
        {
            var encryptedEmail = _fixture.Create<string>();
            encryptedEmail = encryptedEmail + "sdf adf asd";
            var emailWithoutSpaces = encryptedEmail.Replace(' ', '+');
            var decryptedEmail = _fixture.Create<string>();
            var user = _fixture.Create<User>();
            user.IsConfirmed = false;

            _encryptionServiceMock
                .Setup(x => x.DecryptString(emailWithoutSpaces))
                .Returns(decryptedEmail)
                .Verifiable();
            _genericUserRepositoryMock
                .Setup(x =>
                    x.GetByPredicate(x =>
                        x.Email == decryptedEmail))
                .ReturnsAsync(user)
                .Verifiable();
            _genericUserRepositoryMock
                .Setup(
                    x => x.Update(
                        It.Is<User>(x =>
                            x.Id == user.Id
                            && x.IsConfirmed == true)))
                .Verifiable();

            var actual = await _authService.ConfirmUserMail(encryptedEmail);

            Assert.True(actual);
            _encryptionServiceMock.Verify();
            _genericUserRepositoryMock.Verify();
        }
    }
}
