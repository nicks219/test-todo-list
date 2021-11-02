using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RandomSongSearchEngine.Dto;
using RandomSongSearchEngine.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RandomSongSearchEngine.Controllers
{
    [ApiController]
    [Route("account/{action}")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IServiceScopeFactory _scope;

        public LoginController(IServiceScopeFactory serviceScopeFactory, ILogger<LoginController> logger)
        {
            _logger = logger;
            _scope = serviceScopeFactory;
        }

        // при ошибке (запрос с Create контроллера [Authorize]) вернёт
        // https://localhost:5001/Account/Login/?ReturnUrl=%2Fentry%2Fonpostcreate
        // redirected: true
        //
        // при успехе (запрос с Create контроллера [Authorize])вернет
        // https://localhost:5001/entry/onpostcreate
        // redirected: false
        [HttpGet]
        public async Task<ActionResult<string>> Login(string userName)
        {
            var loginModel = new LoginDto(userName);
            var response = await Login(loginModel);
            return response == "[Ok]" ? "[LoginController: Login Ok]" : "[LoginController: Error]";//(ActionResult<string>)BadRequest(response);
        }

        [HttpPost]
        public async Task<string> Login(LoginDto model)
        {
            using var scope = _scope.CreateScope();
            try
            {
                ClaimsIdentity id = await new LoginModel(scope).TryLogin(model);
                if (id != null)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                    return "[Ok]";
                }
                return "[LoginController: Data Error]";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[LoginController: System Error]");
                return "[LoginController: System Error]";
            }
        }

        public async Task<ActionResult<string>> Logout(string returnurl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "[LoginController: Logout]";
        }
    }
}