using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using ProjectManagementApp.BLL.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.BLL.Exceptions;
using Common;
using ProjectManagementApp.DAL.Repositories;
using ProjectManagementApp.BLL.Contracts;
using System.Security.Claims;

namespace ProjectManagementApp.BLL.Services
{
    public class UserService : IUserService
    {
        private IValidationService validations;

        private readonly IUserManager _userManager;

        public UserService(IUserManager userManager, IValidationService validationService)
        {
            _userManager = userManager;
            validations = validationService;
        }


        public async Task<bool> CreateUser(string username, string password, string firstName, string lastName, bool isAdmin)
        {
            validations.CheckUsername(username);

            User user = new User() { UserName = username };

            await _userManager.CreateUserAsync(user, password);

            return true;
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userManager.GetAllAsync();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _userManager.GetUserByUsernameAsync(username);
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.GetUserByIdAsync(id);
        }

        public async Task<bool> IsUserInRole(string userId, string roleName)
        {
            return await _userManager.IsUserInRole(userId, roleName);
        }

        public async Task<bool> DeleteUser(string id)
        {
            User user = await GetUserById(id);
            validations.EnsureUserExist(user);

            await _userManager.DeleteUserAsync(user);

            return true;

        }

        public async Task<bool> EditUser(string id, string newUsername, string newPassword, string newFirstName, string newLastName)
        {
            User user = await GetUserById(id);
            validations.EnsureUserExist(user);
            validations.CheckUsername(newUsername);

            await _userManager.UpdateUserAsync(user, newUsername, newPassword, newFirstName, newLastName);

            return true;
        }
    }
}
