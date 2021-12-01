using Microsoft.AspNetCore.Http;
using ProjectManagementApp.DAL.Entities;
using System.Threading.Tasks;

namespace ProjectManagementApp.WEB.Auth
{
    public interface IAuthProvider
    {
        Task<User> GetCurrentUser(HttpRequest request);
    }
}