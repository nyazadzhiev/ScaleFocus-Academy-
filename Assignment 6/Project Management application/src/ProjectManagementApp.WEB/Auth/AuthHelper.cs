using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using ProjectManagementApp.BLL.Services;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApp.WEB.Auth
{
    public static class AuthHelper
    {
        public static async Task<User> GetCurrentUser(this IUserService userService, HttpRequest request)
        {
            StringValues UsernameAuthHeader;
            StringValues PasswordAuthHeader;

            request.Headers.TryGetValue("Username", out UsernameAuthHeader);
            request.Headers.TryGetValue("Password", out PasswordAuthHeader);
            if (UsernameAuthHeader.Count != 0 && PasswordAuthHeader.Count != 0)
            {
                string username = UsernameAuthHeader.First();
                string password = PasswordAuthHeader.First();
                return await userService.GetUser(username, password);
            }
            return null;
        }
    }
}

