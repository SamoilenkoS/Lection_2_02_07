using Lection_2_BL.DTOs;
using Lection_2_BL.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lection_2_02_07.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IAuthService authService,
            ILogger<UsersController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> SignIn(string login, string password)
        {
            _logger.LogInformation("Trying to login");
            string token = null;
            try
            {
                token = await _authService.SignIn(login, password);
            }
            catch (ArgumentException)
            {
            }

            return !string.IsNullOrEmpty(token) ? Ok(token) : Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserDto userDto)
        {
            return Ok(await _authService.SignUp(userDto));
        }
    }
}
