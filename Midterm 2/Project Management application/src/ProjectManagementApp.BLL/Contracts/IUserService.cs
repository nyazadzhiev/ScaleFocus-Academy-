using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Contracts
{
    public interface IUserService
    {
        Task<bool> CreateUser(string username, string password, string firstName, string lastName, string role);

        Task<User> GetCurrentUserAsync(ClaimsPrincipal principal);

        User GetCurrentUser(ClaimsPrincipal principal);

        Task<bool> IsUserInRole(string userId, string roleId);

        Task<List<User>> GetAllUsers();

        Task<User> GetUserByUsername(string username);

        Task<User> GetUserById(string id);

        Task<bool> DeleteUser(string id);

        Task<bool> EditUser(string id, string newUsername, string newPassword, string newFirstName, string newLastName);
    }
}
