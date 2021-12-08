using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using ProjectManagementApp.BLL.Services;
using ProjectManagementApp.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApp.WEB.Auth
{
    public class AuthProvider
    {
        private readonly IUserService _userService;

        public AuthProvider(IUserService userService)
        {
            _userService = userService;
        }


        public async Task<User> GetCurrentUser(HttpRequest request)
        {
            StringValues UsernameAuthHeader;
            StringValues PasswordAuthHeader;

            request.Headers.TryGetValue("Username", out UsernameAuthHeader);
            request.Headers.TryGetValue("Password", out PasswordAuthHeader);
            if (UsernameAuthHeader.Count != 0 && PasswordAuthHeader.Count != 0)
            {
                string username = UsernameAuthHeader.First();
                string password = PasswordAuthHeader.First();
                return await _userService.GetUser(username, password);
            }
            return null;
        }
    }
}