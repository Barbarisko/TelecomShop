using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TelecomShop.ErrorHandlers;
using TelecomShop.Services;

namespace TelecomShop.Controllers
{
    public struct LoginData
    {
        public string phoneNumber { get; set; }
        public string password {get; set;}
    }

    public struct SignUpData
    {
        public LoginData login { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
    }

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Auth : ControllerBase
    {
        
        private readonly ILogger<Auth> _logger;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IAuthService _authService;

        public Auth(ILogger<Auth> logger, ITokenGenerator tokenGenerator, IAuthService authService)
        {
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _authService = authService;
        }

        [HttpPost]
        public string Login([Required][FromBody] LoginData data)
        {
            var user = _authService.GetUserDataWithAuth(data.phoneNumber, data.password);
            return _tokenGenerator.CreateToken(user);
        }

        [HttpPost]
        public string SignUp([Required][FromBody] SignUpData data)
        {
            var user = _authService.AddUser(data.login.phoneNumber, data.login.password, data.name, data.surname);
            return _tokenGenerator.CreateToken(user);
        }

        [Authorize]
        [HttpGet]
        public string TestLogin()
        {
            return "Hello " + ClaimsHelper.Username(User.Claims) + " " + ClaimsHelper.PhoneNumber(User.Claims);
        }
    }
}
