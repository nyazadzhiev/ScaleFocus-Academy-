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

namespace ProjectManagementApp.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private IValidationService validations;

        public UserService(IUserRepository userRepository, IValidationService validation)
        {
            repository = userRepository;
            validations = validation;
        }

        public async Task<bool> CreateUser(string username, string password, string firstName, string lastName, bool isAdmin, User currentUser)
        {
            validations.CheckUsername(username);

            User newUser = new User()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin
            };

            await repository.AddUser(newUser);
            await repository.SaveChangesAsync();

            return newUser.Id != 0;
        }

        public async Task<User> GetUser(string username, string password)
        {
            return await repository.GetUserAsync(username, password);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await repository.GetUsersAsync();
        }

        public async Task<User> GetUser(string username)
        {
            return await repository.GetUserAsync(username);
        }

        public async Task<User> GetUser(int id)
        {
            return await repository.GetUserAsync(id);
        }

        public async Task<bool> DeleteUser(int id)
        {
            User user = await GetUser(id);
            validations.EnsureUserExist(user);

            repository.DeleteUser(user);
            await repository.SaveChangesAsync();

            return true;

        }

        public async Task<bool> EditUser(int id, string newUsername, string newPassword, string newFirstName, string newLastName)
        {
            User user = await GetUser(id);
            validations.EnsureUserExist(user);

            validations.CheckUsername(newUsername);

            user.Username = newUsername;
            user.Password = newPassword;
            user.FirstName = newFirstName;
            user.LastName = newLastName;


            await repository.SaveChangesAsync();

            return true;
        }
    }
}
