using Lection_2_02_07;
using Lection_2_BL.DTOs;
using Lection_2_BL.Services.AuthService;
using Lection_2_BL.Services.SMTPService;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTesting
{
    public class AuthServiceTests : IClassFixture<IntegrationTestsFactory<Startup>>
    {
        private readonly IntegrationTestsFactory<Startup> _factory;
        private readonly Mock<ISendingBlueSmtpService> _sendingBlueMock;

        public AuthServiceTests(IntegrationTestsFactory<Startup> factory)
        {
            _factory = factory;
            _sendingBlueMock = new Mock<ISendingBlueSmtpService>();
        }

        [Fact]
        public async Task SignIn_WhenSignUpBefore_ShouldReturnToken()
        {
            var services = _factory.WithWebHostBuilder(b => b.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(ISendingBlueSmtpService));

                services.Remove(descriptor);

                services.AddScoped<ISendingBlueSmtpService>((services)
                    =>{ return _sendingBlueMock.Object; });

            })).Services.CreateScope();
            var authService = services.ServiceProvider.GetRequiredService<IAuthService>();
            var hashService = services.ServiceProvider.GetRequiredService<IAuthService>();
            var dto = new UserDto
            {
                Email = "test@gmail.com",
                BirthDate = DateTime.Now,
                FirstName = "asd",
                LastName = "bl bla",
                Password = "qweqwrq123"
            };

            var guid = await authService.SignUp(dto);

            var token = await authService.SignIn(dto.Email, dto.Password);

            Assert.NotNull(token);
        }

        [Fact]
        public async Task SignUp_ShouldAddUserToDb()
        {
            var services = _factory.WithWebHostBuilder(b => b.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(ISendingBlueSmtpService));

                services.Remove(descriptor);

                services.AddScoped<ISendingBlueSmtpService>((services)
                    => { return _sendingBlueMock.Object; });

            })).Services.CreateScope();
            var authService = services.ServiceProvider.GetRequiredService<IAuthService>();
            var hashService = services.ServiceProvider.GetRequiredService<IAuthService>();
            var dto = new UserDto
            {
                Email = "test@gmail.com",
                BirthDate = DateTime.Now,
                FirstName = "asd",
                LastName = "bl bla",
                Password = "qweqwrq123"
            };

            var guid = await authService.SignUp(dto);

            Assert.NotNull(guid);
        }

        [Fact]
        public async Task SignUpTwice_ShouldFailAdd()
        {
            var services = _factory.WithWebHostBuilder(b => b.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(ISendingBlueSmtpService));

                services.Remove(descriptor);

                services.AddScoped<ISendingBlueSmtpService>((services)
                    => { return _sendingBlueMock.Object; });

            })).Services.CreateScope();
            var authService = services.ServiceProvider.GetRequiredService<IAuthService>();
            var hashService = services.ServiceProvider.GetRequiredService<IAuthService>();
            var dto = new UserDto
            {
                Email = "test@gmail.com",
                BirthDate = DateTime.Now,
                FirstName = "asd",
                LastName = "bl bla",
                Password = "qweqwrq123"
            };

            var guid = await authService.SignUp(dto);
            await authService.SignUp(dto);

            Assert.NotNull(guid);
        }
    }
}
