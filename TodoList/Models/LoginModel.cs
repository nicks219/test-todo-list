using DataAccess.TodoContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoList.DataAccess;
using TodoList.Dto;

namespace TodoList.Models
{
    public class LoginModel
    {
        private readonly IServiceScope _scope;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IServiceScope scope)
        {
            _scope = scope;
            _logger = _scope.ServiceProvider.GetRequiredService<ILogger<LoginModel>>();
        }

        public async Task<ClaimsIdentity> TryLogin(LoginDto login)
        {
            try
            {
                if (login.UserName == null)
                {
                    return null;
                }

                await using var repo = _scope.ServiceProvider.GetRequiredService<IRepository>();
                UserEntity user = repo.GetUser(login);
                if (user == null)
                {
                    return null;
                }

                var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, login.UserName) };
                ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[LoginModel: System Error]");
                return null;
            }
        }
    }
}