using Lection_2_BL.DTOs;
using System;
using System.Threading.Tasks;

namespace Lection_2_BL.Services.AuthService
{
    public interface IAuthService
    {
        Task<string> SignIn(string login, string password);
        Task<Guid> SignUp(UserDto user);
    }
}