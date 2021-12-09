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
        public Task<bool> CreateUser(string username, string password, string firstName, string lastName, bool isAdmin);

        public Task<User> GetCurrentUser(ClaimsPrincipal principal);
        Task<bool> IsUserInRole(string userId, string roleId);

        public Task<List<User>> GetAllUsers();

        public Task<User> GetUserByUsername(string username);

        public Task<User> GetUserById(string id);

        public Task<bool> DeleteUser(string id);

        public Task<bool> EditUser(string id, string newUsername, string newPassword, string newFirstName, string newLastName);
    }
}
