using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Contracts
{
    public interface IUserManager
    {
        Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        User GetUser(ClaimsPrincipal claimsPrincipal);
        Task<bool> IsUserInRole(string userId, string roleId);
        Task<User> GetUserByUsernameAsync(string name);
        Task<User> GetUserByIdAsync(string id);
        Task<List<User>> GetAllAsync();
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<List<string>> GetUserRolesAsync(User user);
        Task<IdentityResult> DeleteUserAsync(User user);
        Task<IdentityResult> AddUserToRoleAsync(User user, string role);
        Task<bool> UpdateUserAsync(User user, string newUsername, string newPassword, string newFirstName, string newLastName);
        Task<bool> ValidateUserCredentials(string userName, string password);
    }

}
