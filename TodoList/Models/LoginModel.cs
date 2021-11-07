using DataAccess.TodoContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
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

        public async Task<GenericPrincipal> TryLogin(LoginDto login)
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
                ClaimsIdentity claimsIdentity = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                GenericPrincipal genericPrincipal = new(claimsIdentity, new[] { user.UserStatus.UserStatusName } );

                return genericPrincipal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[LoginModel: System Error]");
                return null;
            }
        }
    }
}